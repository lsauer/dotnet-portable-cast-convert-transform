// <copyright file=mitlicense.md url=http://lsauer.mit-license.org/ >
//             Lo Sauer, 2013-2016
// </copyright>
// <summary>   A tested, generic, portable, runtime-extensible type converter library   </summary
// <language>  C# > 6.0                                                                 </language>
// <version>   3.1.0.2                                                                  </version>
// <author>    Lorenz Lo Sauer; people credited in the sources                          </author>
// <project>   https://github.com/lsauer/dotnet-portable-type-cast                      </project>
namespace Example11
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Reflection;
    using System.IO;
    using System.Linq;
    using System.Collections.Generic;
    using System.Collections;
    using System.Linq.Expressions;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using System.Text.RegularExpressions;

    using Core.TypeCast;
    using Core.Extensions;

    using System.Runtime.CompilerServices;
    using Core.TypeCast.Base;

    using BioCore;
    using BioCore.Converters;

    /// <summary>
    /// Demonstrate library usage in a Bioinformatic context by converting between strongly typed Polynucleotide and / or Aminoacid sequences
    /// </summary>
    /// <remarks>Use for demonstrative purposes only!</remarks> 
    /// <remarks>Note: The code herein serves a demonstrative purpose and not follow an actual implementation of a Bioinformatic library. 
    [Description("Demonstrate library usage in a Bioinformatic context by converting between strongly typed Polynucleotides sequences")]
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine($"Running: {typeof(Program).Namespace} '{typeof(Program).GetCustomAttribute<DescriptionAttribute>()?.Description}'. Press any key...");

            var cc = new ConverterCollection(typeof(Program), typeof(DNA), typeof(RNA))
            {
                AutoReset = true,
                Settings =
                                 new ConverterCollectionSettings
                                 {
                                     ConverterDefaultWrapperOrException
                                             = true
                                 }
            };

            cc.Settings.NumberFormat = new NumberFormatInfo { NumberGroupSeparator = ",", NumberDecimalDigits = 3 };

            var dna = new DNA("gagtgcgccctccccgcacatgcgccctgacagcccaacaatggcggcgcccgcggagtc");
            Console.WriteLine($"{dna.GetType().Name} has Sequence: {dna.CastTo<string>()}");

            var rna = new RNA("GAGUGCGCCCUCCCCGCACAUGCGCCCUGACAGCCCAACAAUGGCGGCGCCCGCGGAGUC");
            Console.WriteLine($"{rna.GetType().Name} has Sequence: {rna.CastTo<string>()}");

            var identical = rna == dna.CastTo<RNA>();
            Console.WriteLine($"{nameof(RNA)}'s are identical: {identical}");

            Console.WriteLine();

            var complementaryDNA = dna.CastTo<cDNA>();
            Console.WriteLine($"{dna.GetType().Name} has Complementary {complementaryDNA.GetType().Name} Sequence: {complementaryDNA.CastTo<string>()}");

            var complementaryRNA = rna.CastTo<cRNA>();

            Console.WriteLine($"{rna.GetType().Name} has Complementary {complementaryRNA.GetType().Name} Sequence: {complementaryRNA.CastTo<string>()}");

            Console.WriteLine();

            double molweightDNA = dna.CastTo<MolecularWeight>();
            Console.WriteLine($"{dna.GetType().Name} has {nameof(MolecularWeight)}: {molweightDNA.CastTo<MolecularWeight>()}");

            double meltingDNA = dna.CastTo<MeltingTemperature>();
            Console.WriteLine($"{dna.GetType().Name} has {nameof(MeltingTemperature)}: {meltingDNA.CastTo<MeltingTemperature>()}");

            Console.WriteLine();

            double molweightRNA = rna.CastTo<MolecularWeight>();
            Console.WriteLine($"{rna.GetType().Name} has {nameof(MolecularWeight)}: {molweightRNA.CastTo<MolecularWeight>()}");

            double meltingRNA = rna.CastTo<MeltingTemperature>();
            Console.WriteLine($"{rna.GetType().Name} has {nameof(MeltingTemperature)}: {meltingRNA.CastTo<MeltingTemperature>()}");

            Console.WriteLine();

            double molweightcRNA = complementaryRNA.CastTo<MolecularWeight>();
            Console.WriteLine($"{complementaryRNA.GetType().Name} has {nameof(MolecularWeight)}: {molweightcRNA.CastTo<MolecularWeight>()}");

            double meltingcRNA = complementaryRNA.CastTo<MeltingTemperature>();
            Console.WriteLine($"{complementaryRNA.GetType().Name} has {nameof(MeltingTemperature)}: {meltingcRNA.CastTo<MeltingTemperature>()}");

            Console.WriteLine();

            double molweightcDNA = complementaryDNA.CastTo<MolecularWeight>();
            Console.WriteLine($"{complementaryDNA.GetType().Name} has {nameof(MolecularWeight)}: {molweightcDNA.CastTo<MolecularWeight>()}");

            double meltingcDNA = complementaryDNA.CastTo<MeltingTemperature>();
            Console.WriteLine($"{complementaryDNA.GetType().Name} has {nameof(MeltingTemperature)}: {meltingcDNA.CastTo<MeltingTemperature>()}");


            // Demonstrate Enum conversion, included within the library (DefaultConverters)

            object polymer = PolymerType.LPolysaccharide;
            PolymerType enumresult = polymer.CastTo(PolymerType.Unknown);
            Console.WriteLine($"The {nameof(PolymerType)} is: {enumresult} (Description: {enumresult.GetDescription()})");

            object polymer2 = PolymerType.RNA;
            PolymerType enumresult2 = polymer2.CastTo<PolymerType>();
            Console.WriteLine($"The {nameof(PolymerType)} is: {enumresult2} (Description: {enumresult2.GetDescription()})");


            // demonstrate simple codon and protein finding / translation
            var seq = @"CCTCAGCGAGGACAGCAAGGGACTAGCCAGGAGGGAGAACAGAAACTCCAGAACATCTTGGAAATAGCTCCCAGAAAAGC
                        AAGCAGCCAACCAGGCAGGTTCTGTCCCTTTCACTCACTGGCCCAAGGCGCCACATCTCCCTCCAGAAAAGACACCATGA
                        GCACAGAAAGCATGATCCGCGACGTGGAACTGGCAGAAGAGGCACTCCCCCAAAAGATGGGGGGCTTCCAGAACTCCAGG
                        CGGTGCCTATGTCTCAGCCTCTTCTCATTCCTGCTTGTGGCAGGGGCCACCACGCTCTTCTGTCTACTGAACTTCGGGGT
                        GATCGGTCCCCAAAGGGATGAGAAGTTCCCAAATGGCCTCCCTCTCATCAGTTCTATGGCCCAGACCCTCACACTCAGAT
                        CATCTTCTCAAAATTCGAGTGACAAGCCTGTAGCCCACGTCGTAGCAAACCACCAAGTGGAGGAGCAGCTGGAGTGGCTG
                        AGCCAGCGCGCCAACGCCCTCCTGGCCAACGGCATGGATCTCAAAGACAACCAACTAGTGGTGCCAGCCGATGGGTTGTA
                        CCTTGTCTACTCCCAGGTTCTCTTCAAGGGACAAGGCTGCCCCGACTACGTGCTCCTCACCCACACCGTCAGCCGATTTG
                        CTATCTCATACCAGGAGAAAGTCAACCTCCTCTCTGCCGTCAAGAGCCCCTGCCCCAAGGACACCCCTGAGGGGGCTGAG
                        CTCAAACCCTGGTATGAGCCCATATACCTGGGAGGAGTCTTCCAGCTGGAGAAGGGGGACCAACTCAGCGCTGAGGTCAA
                        TCTGCCCAAGTACTTAGACTTTGCGGAGTCCGGGCAGGTCTACTTTGGAGTCATTGCTCTGTGAAGGGAATGGGTGTTCA
                        TCCATTCTCTACCCAGCCCCCACTCTGACCCCTTTACTCTGACCCCTTTATTGTCTACTCCTCAGAGCCCCCAGTCTGTA
                        TCCTTCTAACTTAGAAAGGGGATTATGGCTCAGGGTCCAACTCTGTGCTCAGAGCTTTCAACAACTACTCAGAAACACAA
                        GATGCTGGGACAGTGACCTGGACTGTGGGCCTCTCATGCACCACCATCAAGGACTCAAATGGGCTTTCCGAATTCACTGG
                        AGCCTCGAATGTCCATTCCTGAGTTCTGCAAAGGGAGAGTGGTCAGGTTGCCTCTGTCTCAGAATGAGGCTGGATAAGAT
                        CTCAGGCCTTCCTACCTTCAGACCTTTCCAGATTCTTCCCTGAGGTGCAATGCACAGCCTTCCTCACAGAGCCAGCCCCC
                        CTCTATTTATATTTGCACTTATTATTTATTATTTATTTATTATTTATTTATTTGCTTATGAATGTATTTATTTGGAAGGC
                        CGGGGTGTCCTGGAGGACCCAGTGTGGGAAGCTGTCTTCAGACAGACATGTTTTCTGTGAAAACGGAGCTGAGCTGTCCC
                        CACCTGGCCTCTCTACCTTGTTGCCTCCTCTTTTGCTTATGTTTAAAACAAAATATTTATCTAACCCAATTGTCTTAATA
                        ACGCTGATTTGGTGACCAGGCTGTCGCTACATCACTGAACCTCTGCTCCCCACGGGAGCCGTGACTGTAATCGCCCTACG
                        GGTCATTGAGAGAAATAA";
            var dna2 = new DNA(seq);

            Console.WriteLine();

            Console.WriteLine($"List all Codons contained in the DNA Sequence '{nameof(dna2)}, Len: {dna2.Codons.Count()}'");
            Console.WriteLine($"There {(dna2.Letters.Count() % 3 == 0 ? "is" : "is not")} a sequence stump");
            foreach(var codon in dna2 as IEnumerable<Codon>)
            {
                Console.Write($"{codon.Value} \t");
            }

            Console.WriteLine();

            Console.WriteLine($"List the first ten letters in full in the DNA Sequence '{nameof(dna2)}, Len: {dna2.Codons.Count()}'");
            for(int i = 0; i < Math.Min(10, dna2.Letters.Count()); i++)
            {
                Console.Write($"{(Letter)dna2.Sequence[i]} \t");
                //alternatively
                Console.Write($"{((IEnumerable<Letter>)dna2).Skip(i).FirstOrDefault()} \t");
            }

            Console.WriteLine();
            
            //alternative
            var breakcnd = 0;
            foreach(var letter in dna2 as IEnumerable<Letter>)
            {
                if(breakcnd++ >= 10)
                { break; }
                Console.Write($"{letter.FullName} \t");
            }

            Console.WriteLine();

            Console.WriteLine($"List first 20 Nucleotides contained in the DNA Sequence '{nameof(dna2)}, Len: {dna2.Codons.Count()}'");
            breakcnd = 0;
            foreach(var nt in dna2 as IEnumerable<Nucleotide>)
            {

                if(breakcnd++ >= 20)
                { break; }
                Console.Write($"{nt.Name}~");
            }

            Console.WriteLine();

            // demonstrate the default enumeration
            breakcnd = 0;
            var cRna2 = dna2.CastTo<RNA>().Complementary();
            var cRna2Cmp = dna2.CastTo<cRNA>();

            Console.WriteLine($"Complementary sequences are identical? {cRna2} == {cRna2Cmp} ?: { cRna2 == cRna2Cmp}");

            foreach(var nt in cRna2)
            {
                if(breakcnd++ >= 20)
                { break; }
                Console.Write($"{nt.Name}({(Nucleotide)nt})~");
            }

            Console.WriteLine();

            var orfs = dna2.FindORFs(minLength: 15, maxLength: 150);

            // demonstrate transform 
            var seqTrans = orfs.FirstOrDefault().CastTo<Protein>().Transform(functionAlias: nameof(Protein.ToVerbose));
            Console.WriteLine($"{nameof(Protein.ToVerbose)}: {seqTrans}");

            var seqTrans2 = orfs.FirstOrDefault().CastTo<Protein>().Transform<string>(Protein.Transform.ToVerbose);
            Console.WriteLine($"{nameof(Protein.Transform.ToVerbose)}: {seqTrans2}");


            Console.WriteLine();

            // list proteins
            foreach(var orf in orfs)
            {
                Console.WriteLine($"{nameof(ORF)} '{orf}' => Protein: {orf.CastTo<Protein>().Sequence}; ");
                Console.WriteLine($" ....{nameof(Protein.ToConsensus)}, Consensus-Sequence: : {orf.CastTo<Protein>().ToConsensus()} ");
                Console.WriteLine($" ....{nameof(MolecularWeight)}: {orf.CastTo<Protein>().CastTo<MolecularWeight>()} ");
            }

            Console.WriteLine();

            // multiple consecutive default casts of the same type must not yield errors...
            Console.WriteLine($"{nameof(ORF.ToString)} of the first {nameof(ORF)}: " + 
                $" {orfs.FirstOrDefault().CastTo<string>().CastTo<string>().CastTo<string>()}");

            // multiple consecutive converter casts of the same type must not yield errors...
            Console.WriteLine($"{nameof(MolecularWeight)} of the first {nameof(Protein)}: " + 
                $"{orfs.FirstOrDefault().CastTo<Protein>().CastTo<Protein>().CastTo<Protein>().CastTo<MolecularWeight>()}");

            Console.WriteLine($"{nameof(Protein.ToVerbose)} of the first {nameof(Protein)}: " + 
                $" {orfs.FirstOrDefault().CastTo<Protein>().ConvertTo<string>(true)}");

            Console.WriteLine($"{nameof(Protein.ToString)} of the first {nameof(Protein)}: " + 
                $" {orfs.FirstOrDefault().CastTo<Protein>().CastTo<string>()}");


            var proteinToDNA = orfs.FirstOrDefault().CastTo<Protein>().ToCodonCandidates();

            var proteinToConsensusSequence = orfs.FirstOrDefault().CastTo<Protein>().ToConsensus();
            var proteinToConsensusSequence2 = orfs.FirstOrDefault().CastTo<Protein>().Transform<DNA.ConsensusSequence, string>();
            var proteinToConsensusSequence3 = orfs.FirstOrDefault().CastTo<Protein>().Transform<DNA.ConsensusSequence>();

            Console.WriteLine();

            Console.WriteLine($"Total Number of CONVERTERS: {ConverterCollection.CurrentInstance.Count}");

            // Number of physical value converters:
            var cnt1 = ConverterCollection.CurrentInstance.WithTo(typeof(IUnitValue), assignable: true).Count();
            var cnt2 = ConverterCollection.CurrentInstance.WithTo(typeof(IUnit), assignable: true).Count();
            var cnt3 = ConverterCollection.CurrentInstance.WithTo(typeof(IValue), assignable: true).Count();

            Console.WriteLine($"Total Number of Physical CONVERTERS supporting {nameof(IUnitValue)}: ({cnt1} == {cnt2} == {cnt3}): ...");
            foreach(var conv in ConverterCollection.CurrentInstance.WithTo(typeof(IUnitValue), assignable: true))
            {
                Console.WriteLine("\t" + conv);
            }

            // End
            Console.WriteLine("List all Custom Converters? (y/n)");

            var key = Console.ReadKey(true);

            if(key.KeyChar == 'y' || key.KeyChar == 'Y')
            {
                // enumerate all custom converters in "Program"
                foreach(var item in ConverterCollection.CurrentInstance.WithBaseType(typeof(Polymer), assignable: true))
                {
                    Console.WriteLine(item);
                }

                Console.WriteLine($"Total Number of CONVERTERS: {ConverterCollection.CurrentInstance.Count}");

                Console.ReadKey(true);
            }
        }

    }
}