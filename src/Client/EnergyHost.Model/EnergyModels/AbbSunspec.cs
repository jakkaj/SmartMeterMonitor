using System;
using System.Collections.Generic;
using System.Text;

namespace EnergyHost.Model.EnergyModels
{

    public class SolarEdgeSunSpec
    {
        public object PhVphA { get; set; }
        public object PhVphB { get; set; }
        public object PhVphC { get; set; }
        public object PPVphBC { get; set; }
        public double TmpSnk { get; set; }
        public int WH { get; set; }
        public double AphA { get; set; }
        public double Hz { get; set; }
        public int StVnd { get; set; }
        public double DCA { get; set; }
        public double PF { get; set; }
        public object TmpTrns { get; set; }
        public double DCV { get; set; }
        public double DCW { get; set; }
        public double A { get; set; }
        public double VA { get; set; }
        public object PPVphCA { get; set; }
        public int St { get; set; }
        public double PPVphAB { get; set; }
        public object AphB { get; set; }
        public double W { get; set; }
        public object TmpCab { get; set; }
        public object TmpOt { get; set; }
        public object Evt1 { get; set; }
        public object Evt2 { get; set; }
        public int EvtVnd1 { get; set; }
        public object EvtVnd2 { get; set; }
        public object EvtVnd3 { get; set; }
        public int EvtVnd4 { get; set; }
        public double VAr { get; set; }
        public object AphC { get; set; }
    }
    public class ABBSunspec
    {
        public double PhVphA { get; set; }
        public double PhVphB { get; set; }
        public double PhVphC { get; set; }
        public double PPVphBC { get; set; }
        public object TmpSnk { get; set; }
        public int WH { get; set; }
        public double AphA { get; set; }
        public double Hz { get; set; }
        public int StVnd { get; set; }
        public object DCA { get; set; }
        public object PF { get; set; }
        public object TmpTrns { get; set; }
        public object DCV { get; set; }
        public int DCW { get; set; }
        public double A { get; set; }
        public object VA { get; set; }
        public double PPVphCA { get; set; }
        public int St { get; set; }
        public double PPVphAB { get; set; }
        public double AphB { get; set; }
        public int W { get; set; }
        public double TmpCab { get; set; }
        public double TmpOt { get; set; }
        public int Evt1 { get; set; }
        public int Evt2 { get; set; }
        public object EvtVnd1 { get; set; }
        public object EvtVnd2 { get; set; }
        public object EvtVnd3 { get; set; }
        public object EvtVnd4 { get; set; }
        public object VAr { get; set; }
        public double AphC { get; set; }
    }
}
