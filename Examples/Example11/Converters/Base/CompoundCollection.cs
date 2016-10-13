// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.1.0.5                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>
namespace BioCore.Converters
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using Core.TypeCast;
    using Core.TypeCast.Base;
    using System.Text;
    using System.Collections;
    using System.Linq.Expressions;

    public abstract class CompoundCollection<TCompound> : IEnumerable<TCompound>, IQueryable<TCompound>, IRepository<TCompound>
    {
        List<TCompound> value;

        protected CompoundCollection()
        {
            this.value = new List<TCompound>();
        }

        #region IEnumerable support
        public IEnumerator<TCompound> GetEnumerator()
        {
            return ((IEnumerable<TCompound>)this.value).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<TCompound>)this.value).GetEnumerator();
        }
        #endregion

        #region IQueryable<> support
        public Expression Expression
        {
            get
            {
                return this.value.AsQueryable<TCompound>().Expression;
            }
        }

        public Type ElementType
        {
            get
            {
                return this.value.AsQueryable<TCompound>().ElementType;
            }
        }

        public IQueryProvider Provider
        {
            get
            {
                return this.value.AsQueryable<TCompound>().Provider;
            }
        }
        #endregion
        public int Count
        {
            get
            {
                return ((IEnumerable<TCompound>)this.value).Count();
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return true;
            }
        }

        public void Add(TCompound item)
        {
            this.value.Add(item);
        }

        public void AddRange(IEnumerable<TCompound> collection)
        {
            this.value.AddRange(collection);
        }

        public bool Contains(TCompound item)
        {
            return this.value.Contains(item);
        }

        public int IndexOf(TCompound item)
        {
            return this.value.IndexOf(item);
        }

        public abstract TCompound Get(TCompound item);
    }
}