using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if NETFX_CORE
using Windows.UI.Xaml;

#else
using System.Windows;

#endif

namespace BareMVVM.View
{
	public sealed class EventHandlers
	{
		public static EventHandlerList GetList(DependencyObject obj)
		{
			return (EventHandlerList)obj.GetValue(InternalListProperty);
		}

		public static void SetList(DependencyObject obj, EventHandlerList value)
		{
			obj.SetValue(InternalListProperty, value);
		}

		// Using a DependencyProperty as the backing store for List.  This enables animation, styling, binding, etc...
		internal static readonly DependencyProperty InternalListProperty =
			DependencyProperty.RegisterAttached("List",
			                                    typeof(EventHandlerList),
			                                    typeof(EventHandlers),
			                                    new PropertyMetadata(null, ListChangedCallback));

		private static void ListChangedCallback(DependencyObject dependencyObject,
		                                        DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
			EventHandlerList oldHandlerList = dependencyPropertyChangedEventArgs.OldValue as EventHandlerList;

			if (oldHandlerList != null)
			{
				foreach (EventHandlerInstance eventHandlerInstance in oldHandlerList)
				{
					eventHandlerInstance.Disconnect();
				}
			}

			EventHandlerList newList = dependencyPropertyChangedEventArgs.NewValue as EventHandlerList;

			if (newList != null)
			{
				foreach (EventHandlerInstance eventHandlerInstance in newList)
				{
					eventHandlerInstance.Connect(dependencyObject);
				}
			}
		}

		public static string GetAttach(DependencyObject obj)
		{
			return (string)obj.GetValue(InternalAttachProperty);
		}

		public static void SetAttach(DependencyObject obj, string value)
		{
			obj.SetValue(InternalAttachProperty, value);
		}

		public static DependencyProperty AttachProperty
		{
			get { return InternalAttachProperty; }
		}

		// Using a DependencyProperty as the backing store for Attach.  This enables animation, styling, binding, etc...
		internal static readonly DependencyProperty InternalAttachProperty =
			DependencyProperty.RegisterAttached("Attach",
			                                    typeof(string),
			                                    typeof(EventHandlers),
			                                    new PropertyMetadata(null, PropertyChangedCallback));

		private static void PropertyChangedCallback(DependencyObject dependencyObject,
		                                            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
			string attachString = dependencyPropertyChangedEventArgs.NewValue as string;

			if (!string.IsNullOrWhiteSpace(attachString))
			{
				EventHandlerList handlerHelpers = new EventHandlerList();
				string[] attachStrings = attachString.Split(';');

				foreach (string s in attachStrings)
				{
					var trimString = s.Trim();

					if (!string.IsNullOrEmpty(trimString))
					{
						var newHandler = new EventHandlerInstance { Attach = trimString };

						handlerHelpers.Add(newHandler);
					}
				}

				SetList(dependencyObject, handlerHelpers);
			}
		}
	}
}