using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartServiceTests
{
    internal class AddressValidatorTestsData : IEnumerable<object[]>
    {
        readonly string validCity = "ValidCity";
        readonly string invalidCity = "";
        readonly string validCountry = "ValidCountry";
        readonly string invalidCountry = "";
        readonly string validStreet = "123 Street";
        readonly string invalidStreet = "";

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { invalidCity, validCountry, validStreet, false };
            yield return new object[] { validCity, invalidCountry, validStreet, false };
            yield return new object[] { validCity, validCountry, invalidStreet, false };
            yield return new object[] { null , validCountry, validStreet, false };
            yield return new object[] { validCity, null, validStreet, false };
            yield return new object[] { validCity, validCountry, null, false };
            yield return new object[] { validCity, validCountry, validStreet, true };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        
    }
}
