﻿using System.Collections.Generic;
using AsmResolver.DotNet;
using AsmResolver.PE.DotNet.Cil;

namespace Unscrambler.Features.MethodFeatures
{
    public class CalliReplace : IMethodFeature
    {
        private int _count;
        
        public void Process( MethodDefinition method )
        {
            var instr = method.CilMethodBody.Instructions;
            for ( int i = 0; i < instr.Count; i++ )
            {
                if ( instr[i].OpCode != CilOpCodes.Ldftn || instr[i + 1].OpCode != CilOpCodes.Calli )
                    continue;
                
                // Change ldftn to call and remove the calli opcode
                instr[i].OpCode = CilOpCodes.Call;
                instr[i + 1].OpCode = CilOpCodes.Nop;

                _count++;
            }
        }

        public IEnumerable<Summary> GetSummary()
        {
            if ( _count > 0 )
                yield return new Summary( $"Replaced {_count} Calli implementations", Logger.LogType.Success );
        }
    }
}