#if INTERACTIVE // Emulate minimal NUnit in F# Interactive
#r @"..\packages\Foq.1.0\Lib\net40\Foq.dll"

type TestAttribute() = inherit System.Attribute()

module Assert =
    let inline IsTrue(success) = if not success then failwith "Expected true"
    let inline AreEqual(expected, actual) =
        if not (expected = actual) then 
            sprintf "Expected '%A' Actual '%A'" expected actual |> failwith
    let inline Throws<'T when 'T :> exn> (f) =
        let fail () = failwith "Expected %s" typeof<'T>.Name
        try f (); fail () with :? 'T as e -> e | _ -> fail()
#else
module Foq.Usage
open NUnit.Framework
#endif
open Foq

let [<Test>] ``setup a method to always return true`` () =
    // Arrange
    let instance =
        Mock<System.Collections.IList>()
            .Setup(fun x -> <@ x.Contains(any()) @>).Returns(true)
            .Create()
    // Assert
    Assert.IsTrue(instance.Contains("Anything"))

let [<Test>] ``setup a method to return a value based on the argument`` () =
    // Arrange
    let instance =
        Mock<System.Collections.IList>()
            .Setup(fun x -> <@ x.Contains(1) @>).Returns(true)
            .Setup(fun x -> <@ x.Contains(2) @>).Returns(false)
            .Create()
    // Assert
    Assert.AreEqual(true, instance.Contains(1))
    Assert.AreEqual(false,instance.Contains(2))

let [<Test>] ``setup a method to return true or throw predicated on the argument`` () =
    // Arrange
    let instance =
        Mock<System.Collections.Generic.IList<int>>()
            .Setup(fun x -> <@ x.Remove(is(fun i -> i >= 0)) @>).Returns(true)
            .Setup(fun x -> <@ x.Remove(is(fun i -> i <  0)) @>).Raises<System.ArgumentOutOfRangeException>()
            .Create()
    // Assert
    Assert.AreEqual(true, instance.Remove(99))

let [<Test>] ``setup a property to always return 1`` () =
    // Arrange
    let instance =
        Mock<System.Collections.IList>()
            .Setup(fun x -> <@ x.Count @>).Returns(1)
            .Create()
    // Assert
    Assert.AreEqual(1, instance.Count)

let [<Test>] ``setup an item property to return float based on the argument`` () =
    // Arrange
    let instance =
        Mock<System.Collections.Generic.IList<double>>()
            .Setup(fun x -> <@ x.Item(0) @>).Returns(0.0)
            .Setup(fun x -> <@ x.Item(any()) @>).Returns(-1.0)
            .Create()
    // Assert
    Assert.AreEqual(0.0, instance.[0])
    Assert.AreEqual(-1., instance.[1])

let [<Test>] ``setup a method to always raise an exception`` () =
    // Arrange
    let instance =
        Mock<System.IComparable>()
            .Setup(fun x -> <@ x.CompareTo(any()) @>).Raises<System.ApplicationException>()
            .Create()
    // Act
    try instance.CompareTo(1) |> ignore; false with e -> true
    // Assert
    |> Assert.IsTrue

let [<Test>] ``set up an event`` () =
    // Arrange
    let event = Event<_,_>()
    let instance =
        Mock<System.ComponentModel.INotifyPropertyChanged>()
            .SetupEvent(fun x -> <@ x.PropertyChanged @>).Publishes(event.Publish)
            .Create()
    let triggered = ref false
    instance.PropertyChanged.Add(fun x -> triggered := true)
    // Act
    event.Trigger(instance, System.ComponentModel.PropertyChangedEventArgs("X"))
    // Assert
    Assert.IsTrue(!triggered)

let [<Test>] ``setup a method to call a function`` () =
    // Arrange
    let called = ref false
    let instance =
        Mock<System.Collections.Generic.IList<string>>()
            .Setup(fun x -> <@ x.Insert(any(), any()) @>)
                .Calls<int * string>(fun (index,item) -> called := true)
            .Create()
    // Act
    instance.Insert(6, "Six")
    // Assert
    Assert.IsTrue(!called)

let [<Test>] ``verify method is called the specified number of times`` () =
    // Arrange
    let xs = Mock.Of<System.Collections.Generic.IList<int>>()
    // Act
    let _ = xs.Contains(1)
    // Assert
    Mock.Verify(<@ xs.Contains(0) @>, never)
    Mock.Verify(<@ xs.Contains(any()) @>, once)

let [<Test>] ``expect method is called the specified number of times`` () =
    // Arrange
    let xs = Mock.Of<System.Collections.Generic.IList<int>>()
    // Assert
    Mock.Expect(<@ xs.Contains(0) @>, never)
    Mock.Expect(<@ xs.Contains(any()) @>, once)
    // Act
    xs.Contains(1) |> ignore

type ICalculator =
    abstract Push : int -> unit
    abstract Sum : unit -> unit
    abstract Total : int

let [<Test>] ``mock calculator`` () =
    // Arrange
    let instance = 
        Mock<ICalculator>()
            .Setup(fun x -> <@ x.Total @>).Returns(2)
            .Create()
    // Act
    instance.Push(2)
    instance.Sum()
    // Assert
    Assert.AreEqual(2, instance.Total)

let [<Test>] ``implement IList interface members`` () =
    // Arrange
    let xs =
        Mock<System.Collections.Generic.IList<char>>.With(fun xs ->
            <@ xs.Count --> 2 
               xs.Item(0) --> '0'
               xs.Item(1) --> '1'
               xs.Contains(any()) --> true
               xs.RemoveAt(2) ==> System.ArgumentOutOfRangeException()
            @>
        )
    // Assert
    Assert.AreEqual(2, xs.Count)
    Assert.AreEqual('0', xs.Item(0))
    Assert.AreEqual('1', xs.Item(1))
    Assert.IsTrue(xs.Contains('0'))
    Assert.Throws<System.ArgumentOutOfRangeException>(fun () ->
        xs.RemoveAt(2)
    ) |> ignore

#if INTERACTIVE // Run tests in F# Interactive
``setup a method to always return true`` ()
``setup a method to return a value based on the argument`` ()
``setup a method to return true or throw predicated on the argument`` ()
``setup a property to always return 1`` ()
``setup an item property to return float based on the argument`` ()
``setup a method to always raise an exception`` ()
``set up an event`` ()
``setup a method to call a function`` ()
``verify method is called the specified number of times`` ()
``expect method is called the specified number of times`` ()
``mock calculator`` ()
``implement IList interface members`` ()
#endif