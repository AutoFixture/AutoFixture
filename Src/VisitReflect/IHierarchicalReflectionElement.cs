namespace Ploeh.VisitReflect
{
    public interface IHierarchicalReflectionElement
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="visitor"></param>
        /// <returns></returns>
        IHierarchicalReflectionVisitor<T> Accept<T>(IHierarchicalReflectionVisitor<T> visitor);        
    }
}