using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmppCommands
{
    public static class LicenseManager
    {
        private const string lic = @"eJxkkN1ymzAQhV8lk1tPK8CBks5GE9siLq6ntU1wxrlTQATZAoF+HMLT10mc
v+ZmZ3e/PbtnFuY8Y7VmJ10lan1xSu+/aVmYB6rYT/GCTjEslMxtZuIcJ8bm
XAJ678DS0tpw84hdQG85TKw2smIKwx9aMRztqbDUSAXouYaJrBpaP74CLuuT
oxVArwyiinKBNRVMX35w9j0/DL2ww/DbobTJqWFR13DFyCHDnuP6TuAGgL4g
iDVhlcRG2cOuYwFP8bP+zPnxpP8PQMLva2qsYng5WVuthmq9uSkYT7qps/Wv
smS/qLZjf9+d+4qEo3ixG1RO2Q4c0usgCvd6eTUr5Twj9nxMSHxbekm4XS1j
1ft3zUN6dys25VQNA8F3Q5QWzaANdpszsqWharNymv5eiRt3JDYuomsza31v
bntyPd3Jvh0npZsO2oVfWK+bjYq/vV2H3i++ii4AvfsGdHw3/ieAAA==";

        public static void SetLicense()
        {
            Matrix.License.LicenseManager.SetLicense(lic);
        }
    }
}
