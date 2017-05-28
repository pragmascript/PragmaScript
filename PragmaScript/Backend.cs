using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static PragmaScript.SSA;

namespace PragmaScript {
    partial class Backend {

        public Module mod;
        Value intrinsic_memcpy;

        public Backend() {
            mod = new Module();

            // add memcpy
            var ft = new FunctionType(Const.void_t, Const.ptr_t, Const.ptr_t, Const.i32_t, Const.bool_t);
            intrinsic_memcpy = mod.AddFunctionDecl("llvm.memcpy.p0i8.p0i8.i32", ft);
        }


    }
}
