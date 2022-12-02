using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BlockChain.Share
{
    /// <summary>
    /// Interaction logic for Pager.xaml
    /// </summary>
    public partial class Pager : UserControl
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static RoutedEvent FirstPageEvent;
        public static RoutedEvent PreviousPageEvent;
        public static RoutedEvent NextPageEvent;
        public static RoutedEvent LastPageEvent;
        public static RoutedEvent PageRecNumChangedEvent;

        public static readonly DependencyProperty CurrentPageProperty;
        public static readonly DependencyProperty TotalPageProperty;

        public int CurrentPage
        {
            get { return (int)GetValue(CurrentPageProperty); }
            set { SetValue(CurrentPageProperty, value); }
        }

        public int TotalPage
        {
            get { return (int)GetValue(TotalPageProperty); }
            set { SetValue(TotalPageProperty, value); }
        }

        public int CurrentPageRecNum
        {
            get { return _CurrentPageRecNum; }
            set { _CurrentPageRecNum = value; TextBoxRecordNum.Text = value.ToString(); }
        }

        public Pager()
        {
            InitializeComponent();
        }

        static Pager()
        {
            FirstPageEvent = EventManager.RegisterRoutedEvent("FirstPage", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(Pager));
            PreviousPageEvent = EventManager.RegisterRoutedEvent("PreviousPage", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(Pager));
            NextPageEvent = EventManager.RegisterRoutedEvent("NextPage", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(Pager));
            LastPageEvent = EventManager.RegisterRoutedEvent("LastPage", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(Pager));
            PageRecNumChangedEvent = EventManager.RegisterRoutedEvent("PageRecNumChanged", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(Pager));

            //CurrentPageProperty = DependencyProperty.Register("CurrentPage", typeof(string), typeof(Pager), new PropertyMetadata(string.Empty,new PropertyChangedCallback(OnCurrentPageChanged)));
            //TotalPageProperty = DependencyProperty.Register("TotalPage", typeof(string), typeof(Pager), new PropertyMetadata(string.Empty,new PropertyChangedCallback(OnTotalPageChanged)));
            CurrentPageProperty = DependencyProperty.Register("CurrentPage", typeof(int), typeof(Pager), new PropertyMetadata(0, new PropertyChangedCallback(OnCurrentPageChanged)));
            TotalPageProperty = DependencyProperty.Register("TotalPage", typeof(int), typeof(Pager), new PropertyMetadata(0, new PropertyChangedCallback(OnTotalPageChanged)));
        }

        public event RoutedEventHandler FirstPage
        {
            add { AddHandler(FirstPageEvent, value); }
            remove { RemoveHandler(FirstPageEvent, value); }
        }

        public event RoutedEventHandler PreviousPage
        {
            add { AddHandler(PreviousPageEvent, value); }
            remove { RemoveHandler(PreviousPageEvent, value); }
        }

        public event RoutedEventHandler NextPage
        {
            add { AddHandler(NextPageEvent, value); }
            remove { RemoveHandler(NextPageEvent, value); }
        }

        public event RoutedEventHandler LastPage
        {
            add { AddHandler(LastPageEvent, value); }
            remove { RemoveHandler(LastPageEvent, value); }
        }

        public event RoutedEventHandler PageRecNumChanged
        {
            add { AddHandler(PageRecNumChangedEvent, value); }
            remove { RemoveHandler(PageRecNumChangedEvent, value); }
        }

        public static void OnTotalPageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Pager p = d as Pager;

            if(p != null)
            {
                Run rTotal = (Run)p.FindName("rTotal");
                rTotal.Text = e.NewValue.ToString();
            }
        }

        private static void OnCurrentPageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Pager p = d as Pager;

            if(p != null)
            {
                Run rCurrrent = (Run)p.FindName("rCurrent");

                rCurrrent.Text = e.NewValue.ToString();
            }
        }

        private void FirstPageButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(FirstPageEvent, this));
        }

        private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(PreviousPageEvent, this));
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(NextPageEvent, this));
        }

        private void LastPageButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(LastPageEvent, this));
        }


        private int _CurrentPageRecNum = 10;        //要和  TextBoxRecordNum.Text 一样 都是 10

        private void TextBoxRecordNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            //try
            //{
            //    var t = TextBoxRecordNum.Text;
            //   var pr = int.Parse(t);                   //这句话会不会被优化掉
            //    if (pr < 10) {
            //        throw new Exception("Min Value Is 10.");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    TextBoxRecordNum.Text = _CurrentPageRecNum.ToString();
            //    log.Info("", ex);
            //    return;
            //}

        }

        private void TextBoxRecordNum_LostFocus(object sender, RoutedEventArgs e)
        {
            int pr;
            try
            {
                var t = TextBoxRecordNum.Text;
                pr  = int.Parse(t);
                if (pr < 10)
                {
                    throw new Exception("Min Value Is 10.");
                }
            }
            catch (Exception ex)
            {
                TextBoxRecordNum.Text = _CurrentPageRecNum.ToString();
                log.Info("", ex);
                return;
            }

            if (pr != _CurrentPageRecNum)
            {
                _CurrentPageRecNum = pr;
                RaiseEvent(new RoutedEventArgs(PageRecNumChangedEvent, this));
            }
        }
    }
}
