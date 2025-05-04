using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mxProject.Data.Filters
{
    internal static class DbFilterSet
    {
        internal static DbFilterSet<TTarget, TFilter> Create<TTarget, TFilter>(IEnumerable<TFilter>? filters, Func<TFilter, TTarget> getTarget) where TTarget : Enum
        {
            return new DbFilterSet<TTarget, TFilter>(filters ?? Array.Empty<TFilter>(), getTarget);
        }

        internal static readonly DbFilterSet<DbConnectionFilterTargets, IDbConnectionFilter> EmptyConnectionFilters = Create(Array.Empty<IDbConnectionFilter>(), x => x.TargetMethods);
        internal static readonly DbFilterSet<DbTransactionFilterTargets, IDbTransactionFilter> EmptyTransactionFilters = Create(Array.Empty<IDbTransactionFilter>(), x => x.TargetMethods);
        internal static readonly DbFilterSet<DbCommandFilterTargets, IDbCommandFilter> EmptyCommandFilters = Create(Array.Empty<IDbCommandFilter>(), x => x.TargetMethods);
        internal static readonly DbFilterSet<DataParameterCollectionFilterTargets, IDataParameterCollectionFilter> EmptyParametersFilters = Create(Array.Empty<IDataParameterCollectionFilter>(), x => x.TargetMethods);
        internal static readonly DbFilterSet<DataReaderFilterTargets, IDataReaderFilter> EmptyReaderFilters = Create(Array.Empty<IDataReaderFilter>(), x => x.TargetMethods);
    }

    internal class DbFilterSet<TTarget, TFilter> where TTarget : Enum
    {
        internal DbFilterSet(IEnumerable<TFilter> filters, Func<TFilter, TTarget> getTarget)
        {
            int foundTargets = default!;
            Dictionary<TTarget, IList<TFilter>> dic = new();

            var targets = Enum.GetValues(typeof(TTarget)).Cast<TTarget>().ToArray();

            foreach (var filter in filters)
            {
                var target = getTarget(filter);

                for (var i = 0; i < targets.Length; i++)
                {
                    if (target.HasFlag(targets[i]))
                    {
                        if (!dic.TryGetValue(targets[i], out var list))
                        {
                            list = new List<TFilter>();
                            dic.Add(targets[i], list);

                            foundTargets = (int)foundTargets | (int)(object)targets[i];
                        }

                        list.Add(filter);
                    }
                }
            }

            Targets = (TTarget)(object)foundTargets;
            m_Filters = dic.ToDictionary(x => x.Key, x => x.Value.ToArray());
        }

        private readonly IReadOnlyDictionary<TTarget, TFilter[]> m_Filters;

        private static readonly TFilter[] s_EmptyTargets = Array.Empty<TFilter>();

        internal TTarget Targets { get; }

        internal TFilter[] this[TTarget target]
        {
            get
            {
                if (m_Filters.TryGetValue(target, out var filters))
                {
                    return filters;
                }
                return s_EmptyTargets;
            }
        }
    }
}
