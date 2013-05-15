using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ischedule
{
    /// <summary>
    /// 可排序列表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SortableBindingList<T> : BindingList<T>
    {
        ListSortDirection sortDirectionValue;
        PropertyDescriptor sortPropertyValue;

        protected override PropertyDescriptor SortPropertyCore
        {
            get { return sortPropertyValue; }
        }

        protected override ListSortDirection SortDirectionCore
        {
            get { return sortDirectionValue; }
        }

        protected override bool SupportsSortingCore
        {
            get { return true; }
        }

        protected override bool IsSortedCore
        {
            get { return true; }
        }

        protected abstract Comparison<T> GetComparer(PropertyDescriptor prop);

        protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
        {
            List<T> itemsList = (List<T>)this.Items;

            if (itemsList != null)
            {
                if (prop.PropertyType.GetInterface("IComparable") != null)
                {
                    Comparison<T> comparer = GetComparer(prop);

                    if (comparer != null)
                    {
                        itemsList.Sort(comparer);
                        if (direction == ListSortDirection.Descending)
                        {
                            itemsList.Reverse();
                        }
                    }
                }
            }
        }
    }
}