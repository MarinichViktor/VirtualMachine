using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualMachine
{
    public class AsmBuilder
    {
        public static class Register
        {
            public  const String SP = "SP";
            public  const String ARG = "LCL";
            public  const String LCL = "LCL";
            public  const String THIS = "THIS";
            public  const String THAT = "THAT";
            public  const String R0 = "R0";
            public  const String R1 = "R1";
            public  const String R2 = "R2";
            public  const String R3 = "R3";
            public  const String R4 = "R4";
            public  const String R5 = "R5";
            public  const String R6 = "R6";
            public  const String R7 = "R7";
            public  const String R8 = "R8";
            public  const String R9 = "R9";
            public  const String R10 = "R10";
        }

        public static class Command
        {
            public  const String Zero = "0";
            public  const String One = "1";
            public  const String MinusOne = "-1";
            public  const String D = "D";
            public  const String A = "A";
            public  const String M = "M";
            public  const String NotD = "!D";
            public  const String NotA = "!A";
            public  const String NotM = "!M";
            public  const String MinusD = "-D";
            public  const String MinusM = "-M";
            public  const String MinusA = "-A";
            public  const String IncrementM = "M+1";
            public  const String IncrementD = "D+1";
            public  const String IncrementA = "A+1";
            public  const String DecrementM = "M-1";
            public  const String DecrementD = "D-1";
            public  const String DecrementA = "A-1";
            public  const String DPlusA = "D+A";
            public  const String DPlusM = "D+M";
            public  const String DMinusA = "D-A";
            public  const String DMinusM = "D-M";
            public  const String MMinusD = "M-D";
            public  const String AMinusD = "A-D";
            public  const String DAndA = "D&A";
            public  const String DAndM = "D&M";
            public  const String DOrA = "D|A";
            public  const String DOrM = "D|M";
        }

        private StringBuilder _builder = new StringBuilder();
        private Dictionary<SegmentType, String> _segments = new Dictionary<SegmentType, string>
        {
            {SegmentType.Argument, "ARG"},
            {SegmentType.Local, "LCL"},
            {SegmentType.This, "This"},
            {SegmentType.That, "That"},
            // TODO: handle next locations
            // {SegmentType.Static, "LCL"},
            // Pointer,
            // Temp,
        };

        public String Build() => _builder.ToString();
        
        public AsmBuilder PushDOnStack()
        {
            LoadA(Register.SP)
                .AssignA(Command.M)
                .AssignM(Command.D)
                .LoadA(Register.SP)
                .AssignM(Command.IncrementM);

            return this;
        }

        public AsmBuilder PopDFromStack()
        {
            LoadA(Register.SP)
                .AssignM(Command.DecrementM)
                .AssignA(Command.M)
                .AssignD(Command.M);

            return this;
        }

        public AsmBuilder LoadSegmentValueIntoD(SegmentType segment, String index)
        {
            if (_segments.ContainsKey(segment))
            {
                LoadA(_segments[segment])
                    .AssignD(Command.M)
                    .LoadA(index)
                    .AssignA(Command.DPlusA)
                    .AssignD(Command.M);

            } 
            else if (segment == SegmentType.Constant)
            {
                LoadA(index)
                    .AssignD(Command.A);
            }
            else
            {
                throw new NotImplementedException();
            }

            return this;
        }
        public AsmBuilder LoadDIntoSegment(SegmentType segment, String index)
        {
            if (_segments.ContainsKey(segment))
            {
                LoadA(_segments[segment])
                    .AssignD(Command.M)
                    .LoadA(index)
                    .AssignA(Command.DPlusA)
                    .AssignM(Command.D);

            }
            else
            {
                throw new NotImplementedException();
            }

            return this;
        }
        
        public AsmBuilder LoadA(string symbol)
        {
            _builder.AppendLine($@"@{symbol}");
            return this;
        }
        
        public AsmBuilder Label(string symbol)
        {
            _builder.AppendLine($@"({symbol})");
            return this;
        }
        
        public AsmBuilder JGTD()
        {
            _builder.AppendLine("D;JGT");
            return this;
        }
        
        public AsmBuilder JEQD()
        {
            _builder.AppendLine("D;JEQ");
            return this;
        }
        
        public AsmBuilder JLTD()
        {
            _builder.AppendLine("D;JLT");
            return this;
        }

        public AsmBuilder JMP()
        {
            _builder.AppendLine("0;JMP");
            return this;
        }

        public AsmBuilder AssignA(string symbol)
        {
            _builder.AppendLine($@"A={symbol}");
            return this;
        }

        public AsmBuilder AssignM(string symbol)
        {
            _builder.AppendLine($@"M={symbol}");
            return this;
        }
        
        public AsmBuilder AssignD(string symbol)
        {
            _builder.AppendLine($@"D={symbol}");
            return this;
        }

        public AsmBuilder AssignAM(string symbol)
        {
            _builder.AppendLine($@"AM={symbol}");
            return this;
        }

        public AsmBuilder AssignMD(string symbol)
        {
            _builder.AppendLine($@"MD={symbol}");
            return this;
        }

        public AsmBuilder AssignAD(string symbol)
        {
            _builder.AppendLine($@"AD={symbol}");
            return this;
        }

        public AsmBuilder AssignAMD(string symbol)
        {
            _builder.AppendLine($@"AMD={symbol}");
            return this;
        }
    }
}