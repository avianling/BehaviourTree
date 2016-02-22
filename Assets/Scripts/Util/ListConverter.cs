using UnityEngine;
using System.Collections.Generic;

public class ListConverter<From,To> : IListConverter<To> where From : To {

    protected List<From> list;

    public ListConverter(List<From> list)
    {
        this.list = list;
    }

    public int Count
    {
        get
        {
            return list.Count;
        }
    }

    public To this[int i] {
        get
        {
            return (To)list[i];
        }
        set
        {
            list[i] = (From)value;
        }
    }

}

public interface IListConverter<To>
{
    To this[int i]
    {
        get; set;
    }

    int Count { get; }
}