using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarsTask3
{
    class SubsidyCalculation : ISubsidyCalculation
    {
        public event EventHandler<string> OnNotify;
        public event EventHandler<Tuple<string, Exception>> OnException;

        public Charge CalculateSubsidy(Volume volumes, Tariff tariff)
        {
            try
            {
                OnNotify(this, $"Расчёт начат в {DateTime.Now}");

                Validate(volumes, tariff);
                var charge = new Charge
                {
                    HouseId = volumes.HouseId,
                    ServiceId = volumes.ServiceId,
                    Month = volumes.Month,
                    Value = volumes.Value * tariff.Value
                }; 

                OnNotify(this, $"Расчёт успешно завершён в {DateTime.Now}");
                return charge;
            }
            catch(Exception ex)
            {
                OnException(this, Tuple.Create(ex.Message, ex));
                throw;
            }
        }

        private void Validate(Volume volumes, Tariff tariff)
        {
            if (volumes.HouseId != tariff.HouseId)
            {
                throw new Exception("HouseId у тарифа и объёма не совпадают");
            }

            if (volumes.ServiceId != tariff.ServiceId)
            {
                throw new Exception("ServiceId у тарифа и объёма не совпадают");
            }

            if (
                volumes.Month.Month < tariff.PeriodBegin.Month || 
                volumes.Month.Month > tariff.PeriodEnd.Month
               )
            {
                throw new Exception("Значение поля volumes.Month не входит в период тарифа");
            }

            if (volumes.Value < 0)
            {
                throw new Exception("Значение объема меньше нуля");
            }

            if (tariff.Value <= 0)
            {
                throw new Exception("Значение тарифа меньше или равно нулю");
            }
        }
    }
}
