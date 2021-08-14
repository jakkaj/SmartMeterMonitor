using System;
using System.Collections.Generic;
using System.Text;

namespace EnergyHost.Model.EnergyModels
{

    public class SolarEdgeAPIEnergy
    {
        public class Value
        {
            public string date { get; set; }
            public object value { get; set; }
        }

        public class Meter
        {
            public List<Value> values { get; set; }
            public string type { get; set; }
        }

        public string timeUnit { get; set; }
        public List<Meter> meters { get; set; }
        public string unit { get; set; }

    }

    public class LOAD
    {
        public string status { get; set; }
        public double currentPower { get; set; }
    }

    public class PV
    {
        public string status { get; set; }
        public double currentPower { get; set; }
    }

    public class Connection
    {
        public string to { get; set; }
        public string from { get; set; }
    }

    public class GRID
    {
        public string status { get; set; }
        public double currentPower { get; set; }
    }

    public class SiteCurrentPowerFlow
    {
        public LOAD LOAD { get; set; }
        public PV PV { get; set; }
        public List<Connection> connections { get; set; }
        public GRID GRID { get; set; }
        public int updateRefreshRate { get; set; }
        public string unit { get; set; }
    }

    public class SolarEdgeMeter
    {

        public double A { get; set; }
        public double PhVphA { get; set; }
        public double PhVphB { get; set; }
        public double PhVphC { get; set; }
        public double PhVphAB { get; set; }
        public int TotWhImp { get; set; }
        public object TotVArhExpQ4PhA { get; set; }
        public object TotVArhExpQ4PhC { get; set; }
        public object TotVArhExpQ4PhB { get; set; }
        public double PFphC { get; set; }
        public double PhV { get; set; }
        public double PFphA { get; set; }
        public double PhVphCA { get; set; }
        public object TotVAhExpPhA { get; set; }
        public int WphC { get; set; }
        public int WphB { get; set; }
        public object TotVArhImpQ1 { get; set; }
        public object TotVArhImpQ2 { get; set; }
        public int Evt { get; set; }
        public int TotWhExpPhA { get; set; }
        public object TotWhExpPhB { get; set; }
        public object TotWhExpPhC { get; set; }
        public double W { get; set; }
        public double PF { get; set; }
        public double PPV { get; set; }
        public object TotVArhExpQ3PhA { get; set; }
        public object TotVArhExpQ3PhB { get; set; }
        public int WphA { get; set; }
        public double PFphB { get; set; }
        public object TotVAhImp { get; set; }
        public double Hz { get; set; }
        public double PhVphBC { get; set; }
        public int VAphA { get; set; }
        public int VAphB { get; set; }
        public int VAphC { get; set; }
        public object TotVArhExpQ4 { get; set; }
        public object TotVAhImpPhA { get; set; }
        public int VA { get; set; }
        public object TotVAhExpPhB { get; set; }
        public int TotWhExp { get; set; }
        public double AphB { get; set; }
        public double AphA { get; set; }
        public int VAR { get; set; }
        public object TotVArhImpQ2PhB { get; set; }
        public object TotVArhImpQ2PhC { get; set; }
        public object TotWhImpPhC { get; set; }
        public object TotVArhImpQ2PhA { get; set; }
        public int TotWhImpPhA { get; set; }
        public object TotVArhExpQ3PhC { get; set; }
        public int VARphC { get; set; }
        public int VARphB { get; set; }
        public int VARphA { get; set; }
        public object TotVArhExpQ3 { get; set; }
        public object TotVAhExpPhC { get; set; }
        public object TotVAhImpPhB { get; set; }
        public object TotVAhImpPhC { get; set; }
        public object TotWhImpPhB { get; set; }
        public object TotVAhExp { get; set; }
        public double AphC { get; set; }
        public object TotVArhImpQ1PhC { get; set; }
        public object TotVArhImpQ1PhB { get; set; }
        public object TotVArhImpQ1PhA { get; set; }

    }

    public class SolarEdgeSunSpec
    {
        public SiteCurrentPowerFlow siteCurrentPowerFlow { get; set; }
        public SolarEdgeAPIEnergy energyDetails { get; set; }
        public SolarEdgeMeter meter { get; set; }
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
