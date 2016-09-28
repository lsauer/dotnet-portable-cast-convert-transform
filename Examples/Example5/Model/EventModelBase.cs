// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.1.0.2                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>
namespace Example5.Model
{
    using System;
    using System.Drawing;
    using Core.Singleton;
    using System.ComponentModel;

    public class EventModelBase
    {
        public enum Status
        {
            None,
            Started,
            Busy,
            Completed
        }
        public delegate void StatusChangedEventHandler(object sender, Status e);
        public event ProgressChangedEventHandler ProgressChanged;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event StatusChangedEventHandler StatusChanged;

        public virtual void OnProgressChanged(ProgressChangedEventArgs e)
        {
            this.ProgressChanged?.Invoke(this, e);
        }

        public virtual void OnStatusChanged(Status e)
        {
            this.StatusChanged?.Invoke(this, e);
        }
    }
}
