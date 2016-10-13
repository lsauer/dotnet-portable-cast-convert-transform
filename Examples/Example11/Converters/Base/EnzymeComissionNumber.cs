// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.1.0.2                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>
namespace BioCore.Converters
{
    using System.Runtime.InteropServices;

    using Core.TypeCast;

    [StructLayout(LayoutKind.Sequential)]
    public struct EnzymeComissionNumber
    {
        public int ReactionClass;
        public int ReactionType;
        public int ReactionSubType;
        public int CoFactors;

        /// <summary>
        /// Creates a new instance of <see cref="EnzymeComissionNumber"/>, according to the IUPAC nomenclature, outlined here: 
        /// http://www.chem.qmul.ac.uk/iubmb/nomenclature/
        /// </summary>
        /// <param name="enzymeComissionNumber"></param>
        /// <param name="numberSeparator"></param>
        public EnzymeComissionNumber(string enzymeComissionNumber, char numberSeparator = '.')
        {
            var numbers = enzymeComissionNumber.ConvertTo<int[]>(numberSeparator);
            this.ReactionClass = numbers.Length > 0 ? numbers[0] : 0;
            this.ReactionType = numbers.Length > 1 ? numbers[1] : 0;
            this.ReactionSubType = numbers.Length > 2 ? numbers[2] : 0;
            this.CoFactors = numbers.Length > 3 ? numbers[3] : 0;
        }

        /// <summary>
        /// Creates a new instance of <see cref="EnzymeComissionNumber"/>, according to the IUPAC nomenclature, outlined here: 
        /// </summary>
        /// <param name="reactionClass"></param>
        /// <param name="reactionType"></param>
        /// <param name="reactionSubType"></param>
        /// <param name="coFactors"></param>
        public EnzymeComissionNumber(int reactionClass = 0, int reactionType = 0, int reactionSubType = 0, int coFactors = 0)
        {
            this.ReactionClass = reactionClass;
            this.ReactionType = reactionType;
            this.ReactionSubType = reactionSubType;
            this.CoFactors = coFactors;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return  $@" EC Number:[{nameof(this.ReactionClass)}:{this.ReactionClass}; {nameof(this.ReactionType)}:{this.ReactionType}; " +
                    $@"{nameof(this.ReactionSubType)}:{this.ReactionSubType}; {nameof(this.CoFactors)}:{this.CoFactors}]";
        }
    }
}