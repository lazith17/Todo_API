using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Todo_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CreditCardController : ControllerBase
    {
        [HttpGet("{crediCardNo}")]
        public bool IsValid(string crediCardNo)
        {
            return IsValidCreditcrediCardNo(crediCardNo);
        }

        static bool IsValidCreditcrediCardNo(string crediCardNo)
        {
            int[] crediCardNoArr = new int[crediCardNo.Length];

            for (int i = 0; i < crediCardNo.Length; i++)
            {
                crediCardNoArr[i] = int.Parse(crediCardNo[i].ToString());
            }

            for (int i = crediCardNoArr.Length - 2; i >= 0; i -= 2)
            {
                crediCardNoArr[i] = crediCardNoArr[i] * 2;

                if (crediCardNoArr[i] > 9)
                {
                    crediCardNoArr[i] = crediCardNoArr[i] - 9;
                }
            }

            int sum = 0;

            for (int i = 0; i < crediCardNoArr.Length; i++)
            {
                sum += crediCardNoArr[i];
            }

            if (sum % 10 == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
