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

    public class MonomerCollection<TMonomer> : CompoundCollection<TMonomer>
        where TMonomer : IMonomer
    {
        protected IDictionary<char, TMonomer> cache;

        public MonomerCollection()
            : base()
        {
            Cache();
        }

        // create lookup cache
        protected void Cache()
        {
            this.cache = new Dictionary<char, TMonomer>();
            foreach(var item in this)
            {
                cache.Add(item.Letter, item);
            }
        }

        public TMonomer this[Letter index]
        {
            get
            {
                return cache[index];
            }
        }

        #region IRepository support
        public override TMonomer Get(TMonomer item)
        {
            return this[item.Letter];
        }
        #endregion

    }
}