using System;
using System.Collections.Generic;
using System.Linq;


class SimpTabel
{
    public Drob[,] tabl;
    public uint[] freePerm;
    public uint[] bazPerm;
    public uint step = 0;

    public uint colPerm;
    public uint colOgr;

    public Drob[] func;

    public SimpTabel(uint colPerm, uint colOgr)
    {
        this.colOgr = colOgr;
        this.colPerm = colPerm;

        freePerm = new uint[colPerm];
        for (uint i = 1; i <= colPerm; i++)
        {
            freePerm[i - 1] = i;
        }

        bazPerm = new uint[colOgr];
        for (uint i = 1; i <= colOgr; i++)
        {
            bazPerm[i - 1] = i + colPerm - 1;
        }

        tabl = new Drob[colOgr, colPerm];
        func = new Drob[colPerm];
    }

    public SimpTabel(SimpTabel tabl)
    {
        this.freePerm = tabl.freePerm.ToArray();
        this.bazPerm = tabl.bazPerm.ToArray();
        this.func = tabl.func.ToArray();
        this.step = tabl.step + 1;

        this.colOgr = tabl.colOgr;
        this.colPerm = tabl.colPerm;

        this.tabl = new Drob[colOgr, colPerm];
        for (int i = 0; i < tabl.colOgr; i++)
        {
            for (int j = 0; j < tabl.colPerm; j++)
            {
                this.tabl[i, j] = new Drob(tabl.tabl[i, j]);
            }
        }
    }

    public static void InitVspomogLastLine(SimpTabel tabl){
        for (int j = 0; j < tabl.colPerm; j++)
		{
			tabl.tabl[tabl.colOgr - 1, j] = new Drob(0, 1);
			for (int i = 0; i < tabl.colOgr - 1; i++)
			{
				tabl.tabl[tabl.colOgr - 1, j] += tabl.tabl[i, j];
			}
			tabl.tabl[tabl.colOgr - 1, j] = -tabl.tabl[tabl.colOgr - 1, j];
		}
    }

    //s - столбец r стока
    public static SimpTabel SimplStep(SimpTabel tabl, uint s, uint r)
    {
        SimpTabel ret = new SimpTabel(tabl);

        Drob razEl = 1 / tabl.tabl[s, r];

        for (int j = 0; j < ret.colPerm; j++)
        {
            ret.tabl[s, j] *= razEl;
        }

        for (int i = 0; i < ret.colOgr; i++)
        {
            ret.tabl[i, r] *= -razEl;
        }

        for (int i = 0; i < ret.colOgr; i++)
        {
            if (i == s)
                continue;
            for (int j = 0; j < ret.colPerm; j++)
            {
                if (j == r)
                    continue;

                ret.tabl[i, j] = tabl.tabl[i, j] - tabl.tabl[i, r] * ret.tabl[s, j];
            }
        }

        ret.tabl[s, r] = razEl;

        ret.bazPerm[s] = tabl.freePerm[r];
        ret.freePerm[r] = tabl.bazPerm[s];

        return ret;
    }

    public static (uint strk, uint stlb)[] SimplOporns(SimpTabel tabl)
    {
        List<(uint strk, uint stlb)> ret = new List<(uint strk, uint stlb)>();

        for (uint j = 0; j < tabl.colPerm - 1; j++)
        {
            if (tabl.tabl[tabl.colOgr - 1, j] < Drob.Zero)
            {
                Drob coaf = null;
                int sameOporsCounter = 0;
                //(uint strk, uint stlb) opor = (uint.MaxValue, uint.MaxValue);

                for (uint i = 0; i < tabl.colOgr - 1; i++)
                {
                    if (tabl.tabl[i, j] > Drob.Zero)
                    {
                        Drob new_coaf = tabl.tabl[i, tabl.colPerm - 1] / tabl.tabl[i, j];
                        if (coaf == null || coaf > new_coaf)
                        {
                            coaf = new_coaf;
                           // opor = (i, j);

                            ret.RemoveRange(ret.Count - sameOporsCounter , sameOporsCounter);

                            ret.Add((i, j));
                            sameOporsCounter = 1;
                        }else if (coaf == new_coaf){
                             ret.Add((i, j));
                             sameOporsCounter++;
                        }
                    }
                }

                if (coaf == null)
                {
                    throw new Exception("Нет опорных. Функция уходит в бесконечность.");
                }
            }
        }

        return ret.ToArray();
    }

    public static SimpTabel VspomogToOsnov(SimpTabel tbl){
        SimpTabel ret = new SimpTabel(tbl);

        

        return null;
    }

}


