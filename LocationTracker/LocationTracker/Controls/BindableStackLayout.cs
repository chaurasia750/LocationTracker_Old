using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using Xamarin.Forms;

namespace LocationTracker.Controls
{
    public class BindableStackLayout : StackLayout
    {

        public static readonly BindableProperty ItemsProperty =
        BindableProperty.Create(nameof(Items), typeof(ObservableCollection<View>), typeof(BindableStackLayout), null,
            propertyChanged: (b, o, n) =>
            {
                (n as ObservableRangeCollection<View>).CollectionChanged += (coll, arg) =>
                {
                    switch (arg.Action)
                    {
                        case NotifyCollectionChangedAction.Add:
                            foreach (var v in arg.NewItems)
                                (b as BindableStackLayout).Children.Add((View)v);
                            break;
                        case NotifyCollectionChangedAction.Remove:
                            foreach (var v in arg.NewItems)
                                (b as BindableStackLayout).Children.Remove((View)v);
                            break;
                        case NotifyCollectionChangedAction.Move:
                            //Do your stuff
                            break;
                        case NotifyCollectionChangedAction.Replace:
                            //Do your stuff
                            break;
                    }
                };
            });


        public ObservableRangeCollection<View> Items
        {
            get { return (ObservableRangeCollection<View>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }
    }
}
