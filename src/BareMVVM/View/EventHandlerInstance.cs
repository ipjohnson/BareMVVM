﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
#if NETFX_CORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

#else
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

#endif

namespace BareMVVM.View
{
	public sealed class EventHandlerInstance
	{
		private static readonly string supplemental = typeof(EventHandlerInstance).FullName;
		private static readonly MethodInfo genericConnectMethod;

		static EventHandlerInstance()
		{
			genericConnectMethod =
				typeof(EventHandlerInstance).GetTypeInfo().GetDeclaredMethod("GenericConnectHandler");
		}

		internal enum CallingParameterType
		{
			Sender,
			Args,
			DataContext,
			Literal,
		}

		internal struct CallingParameter
		{
			internal CallingParameterType ParameterType { get; set; }

			internal string PropertyName { get; set; }

			internal string Literal { get; set; }
		}

		private string attach;
		private string eventName;
		private string methodName;
		private List<CallingParameter> parameters;
		private WeakReference elementRef;

		public EventHandlerInstance()
		{
		}

		public string Attach
		{
			get { return attach; }
			set
			{
				attach = value;

				ProcessHandlerMessage(attach);
			}
		}

		private void ProcessHandlerMessage(string handlerMessage)
		{
			parameters = new List<CallingParameter>();

			handlerMessage = handlerMessage.Trim();
			handlerMessage = handlerMessage.Replace("[", "");
			handlerMessage = handlerMessage.Replace("]", "");

			if (string.IsNullOrWhiteSpace(handlerMessage))
			{
				return;
			}

			int equalsIndex = handlerMessage.IndexOf('=');
			string callString = handlerMessage;

			if (equalsIndex >= 0)
			{
				eventName = handlerMessage.Substring(0, equalsIndex).Trim();
				callString = handlerMessage.Substring(equalsIndex + 1);

				if (eventName.StartsWith("Event ", StringComparison.OrdinalIgnoreCase))
				{
					eventName = eventName.Substring("Event ".Length);
				}
			}

			callString = callString.TrimStart('>');

			if (string.IsNullOrEmpty(callString))
			{
				return;
			}

			ParseCallString(callString);
		}

		private void ParseCallString(string callString)
		{
			int leftP = callString.IndexOf('(');
			int rightP = callString.IndexOf(')');

			if (leftP > 0 && rightP > leftP)
			{
				methodName = callString.Substring(0, leftP).Trim();

				if (rightP - leftP > 1)
				{
					string parameterString = callString.Substring(leftP + 1, (rightP - leftP) - 1);

					foreach (string arg in parameterString.Split(','))
					{
						string tempArg = arg.Trim();

						if (tempArg.StartsWith("$") && tempArg.Length > 1)
						{
							CallingParameter parameter = new CallingParameter();
							string[] splitArgs = tempArg.TrimStart('$').Split('.');

							switch (splitArgs[0].ToLower())
							{
								case "datacontext":
									parameter.ParameterType = CallingParameterType.DataContext;
									break;

								case "source":
								case "sender":
									parameter.ParameterType = CallingParameterType.Sender;
									break;

								case "arg":
								case "args":
								case "eventarg":
								case "eventargs":
									parameter.ParameterType = CallingParameterType.Args;
									break;

								default:
									parameter.ParameterType = CallingParameterType.Literal;
									parameter.Literal = tempArg;
									break;
							}

							if (splitArgs.Length > 1 && parameter.ParameterType != CallingParameterType.Literal)
							{
								parameter.PropertyName = splitArgs[1];
							}

							parameters.Add(parameter);
						}
						else
						{
							parameters.Add(new CallingParameter
								               {
									               Literal = tempArg,
									               ParameterType = CallingParameterType.Literal
								               });
						}
					}
				}
			}
			else
			{
                // TODO: logging
				//Logger.Error("There was a problem parsing the message string: " + callString, supplemental);
			}
		}

		public void Connect(DependencyObject element)
		{
			elementRef = new WeakReference(element);

			if (string.IsNullOrEmpty(eventName))
			{
				if (element is Button || element is ToggleButton)
				{
					eventName = "Click";
				}
				else if (element is Selector)
				{
					eventName = "SelectionChanged";
				}
				else if (element is TextBox)
				{
					eventName = "TextChanged";
				}
#if NETFX_CORE
				else if (element is ToggleSwitch)
				{
					eventName = "Toggled";
				}
#endif
				else
				{
					eventName = "Loaded";
				}
			}

			try
			{
				EventInfo eventInfo = getEventInfo(element.GetType());

				if (eventInfo != null)
				{
					MethodInfo newEventType =
						genericConnectMethod.MakeGenericMethod(eventInfo.EventHandlerType);
					MemberInfo connectMethod = GetType().GetTypeInfo().GetDeclaredMethod("eventHandler");

					newEventType.Invoke(this, new object[] { element, eventInfo, this, connectMethod });
				}
				else
				{
                    // TODO: logging
                    //Logger.Error(string.Format("Could not find event {0} on type {1}", eventName, element.GetType().FullName),
                    //             supplemental);
                }
            }
			catch (Exception exp)
            {
                // TODO: logging
                //Logger.Error("Exception thrown while connecting event", supplemental, exp);
            }
        }

		internal void GenericConnectHandler<T>(
			DependencyObject d, EventInfo eventInfo, object target, MethodInfo methodInfo) where T : class
		{
			try
			{
#if DOT_NET
				Delegate handlerDelegate = Delegate.CreateDelegate(eventInfo.EventHandlerType, target, methodInfo);

				eventInfo.AddEventHandler(d, handlerDelegate);
#else
				T handlerDelegate = methodInfo.CreateDelegate(eventInfo.EventHandlerType, target) as T;

				Func<T, EventRegistrationToken> add =
					(a) => { return (EventRegistrationToken)eventInfo.AddMethod.Invoke(d, new object[] { a }); };

				Action<EventRegistrationToken> remove =
					(a) => eventInfo.RemoveMethod.Invoke(eventInfo, new object[] { a });

				WindowsRuntimeMarshal.AddEventHandler(add, remove, handlerDelegate);
#endif
			}
			catch (Exception exp)
            {
                // TODO: logging
                //Logger.Error("Exception thrown while setting event handler", typeof(EventHandlerInstance).FullName, exp);
            }
        }

		public void Disconnect()
		{
			DependencyObject d = elementRef.Target as DependencyObject;

			if (d != null)
			{
				try
				{
					EventInfo eventInfo = getEventInfo(d.GetType());

					if (eventInfo != null)
					{
						RoutedEventHandler handlerDelegate =
							GetType().GetTypeInfo().GetDeclaredMethod("eventHandler").
							          CreateDelegate(eventInfo.EventHandlerType, this) as RoutedEventHandler;

						Action<EventRegistrationToken> remove =
							(a) => eventInfo.RemoveMethod.Invoke(eventInfo, new object[] { a });

						WindowsRuntimeMarshal.RemoveEventHandler(remove, handlerDelegate);
					}
				}
				catch (Exception exp)
                {
                    // TODO: logging
                    //Logger.Error("Exception thrown while finding event handler", supplemental, exp);
				}
			}
		}

		private EventInfo getEventInfo(Type type)
		{
			EventInfo eventInfo = type.GetRuntimeEvent(eventName);

			if (eventInfo == null)
			{
				Type baseType = type.GetTypeInfo().BaseType;

				if (baseType != null)
				{
					return getEventInfo(type.GetTypeInfo().BaseType);
				}
			}

			return eventInfo;
		}

#if DOT_NET
		private void eventHandler(object sender, EventArgs args)
#else
		private void eventHandler(object sender, object args)
#endif
		{
			FrameworkElement originalSender = sender as FrameworkElement;
			FrameworkElement tempSender = originalSender;

			if (originalSender != null)
			{
				object[] callParameters = ProcessParameters(originalSender, args);

				object dataContext;
				MethodInfo method = FindMethodOnContext(originalSender, callParameters, out dataContext);

				if (method != null)
				{
					try
					{
						method.Invoke(dataContext, callParameters);
					}
					catch (Exception exp)
                    {
                        // TODO: logging
                        //Logger.Error("Exception thrown while trying to deliver message: " + methodName, supplemental, exp);
					}
				}
				else
                {
                    // TODO: logging
                    //Logger.Error("Could not find method on context: " + methodName);
				}
			}
		}

		private object[] ProcessParameters(FrameworkElement originalSender, object args)
		{
			List<object> returns = new List<object>();

			foreach (CallingParameter callingParameter in parameters)
			{
				object currentValue = null;

				switch (callingParameter.ParameterType)
				{
					case CallingParameterType.Literal:
						currentValue = callingParameter.Literal;
						break;
					case CallingParameterType.DataContext:
						currentValue = originalSender.DataContext;
						break;
					case CallingParameterType.Args:
						currentValue = args;
						break;
					case CallingParameterType.Sender:
						currentValue = originalSender;
						break;
				}

				if (callingParameter.PropertyName != null && currentValue != null)
				{
					PropertyInfo propInfo =
						currentValue.GetType().GetTypeInfo().GetDeclaredProperty(callingParameter.PropertyName);

					if (propInfo != null)
					{
						currentValue = propInfo.GetValue(currentValue);
					}
					else
                    {
                        // TODO: logging
                        //Logger.Error(
                        //	string.Format("Could not find property {0} on type {1}",
                        //	              callingParameter.PropertyName,
                        //	              currentValue.GetType().FullName),
                        //	supplemental);
                    }
                }

				returns.Add(currentValue);
			}

			return returns.ToArray();
		}

		private MethodInfo FindMethodOnContext(FrameworkElement currentElement,
		                                       object[] callParameters,
		                                       out object dataContext)
		{
			MethodInfo method = null;
			object currentContext = null;

			dataContext = null;

			while (currentElement != null && method == null)
			{
				if (currentElement.DataContext != null && currentElement.DataContext != currentContext)
				{
					currentContext = currentElement.DataContext;

					foreach (MethodInfo declaredMethod in currentContext.GetType().GetTypeInfo().GetDeclaredMethods(methodName))
					{
						if (declaredMethod.GetParameters().Count() == callParameters.Length)
						{
							method = declaredMethod;
							dataContext = currentContext;
							break;
						}
					}
				}

				currentElement = VisualTreeHelper.GetParent(currentElement) as FrameworkElement;
			}

			return method;
		}
	}
}