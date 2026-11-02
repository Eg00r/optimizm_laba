using System;
using System.Collections.Generic;
using System.Linq;


class SimpTabel
{
    public Drob[,] tabl;
    public uint[] freePerm;
    public uint[] bazPerm;
    public uint step;

    public uint colPerm;

    public uint colOgr;

    public SimpTabel(){
    }

    public SimpTabel(Drob[,] tabl, uint[] freePerm, uint[] bazPerm, uint step, uint colPerm, uint colOgr)
    {
        this.tabl = tabl;
        this.freePerm = freePerm;
        this.bazPerm = bazPerm;
        this.step = step;
        this.colOgr = colOgr;
        this.colPerm = colPerm;
    }

    public SimpTabel(SimpTabel tabl)
    {
        this.freePerm = tabl.freePerm.ToArray();
        this.bazPerm = tabl.bazPerm.ToArray();
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

    //s - столбец r стока
    public static SimpTabel SimplStep(SimpTabel tabl, uint s, uint r)
    {
        SimpTabel ret = new SimpTabel(tabl);
        ret.step++;

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

        for (uint j = 0; j < tabl.colPerm-1; j++)
        {
            if (tabl.tabl[tabl.colOgr - 1, j] < Drob.Zero)
            {
                Drob coaf = null;
                (uint strk, uint stlb) opor = (uint.MaxValue, uint.MaxValue);

                for (uint i = 0; i < tabl.colOgr - 1; i++)
                {
                    if (tabl.tabl[i, j] > Drob.Zero)
                    {
                        Drob new_coaf = tabl.tabl[i, tabl.colPerm - 1] / tabl.tabl[i, j];
                        if (coaf == null || coaf > new_coaf)
                        {
                            coaf = new_coaf;
                            opor = (i, j);
                        }
                    }
                }


                if (coaf == null)
                {
                    throw new Exception("Нет опорных. Функция уходит в бесконечность.");
                }

                ret.Add(opor);
            }
        }

        return ret.ToArray();
    }
}


