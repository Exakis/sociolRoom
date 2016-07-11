using System;

namespace TwitterListener
{
    public class GenericEventArgs<T>:EventArgs
    {
        public T Data { get; set; }
    }
}