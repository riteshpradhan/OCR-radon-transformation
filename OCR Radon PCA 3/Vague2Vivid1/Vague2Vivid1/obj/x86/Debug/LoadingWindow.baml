   M S B A M L   `   `   `  ����  Vague2Vivid1     Vague2Vivid1.LoadingWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  LoadingWindow��5   	   $	��300��6   $	��300��6,    � WindowStyle$
 None=�5   	    � WindowStartupLocation$ CenterScreen=�5   	    � 
ResizeMode$ NoResize=�5       � ShowInTaskbar .   � Topmost .  � 	Resources 5         Storyboard1 g� RepeatBehavior g� 	BeginTime	 �� 
TargetName
 �� TargetProperty O� KeyTime O� Value��%  &	        �� 5   
   $ Forever��6)   .��5      U� $ 00:00:00X�6,   $	 ellipse��6A   $V
 P(UIElement.RenderTransform).(TransformGroup.Children)[2].(RotateTransform.Angle)�6a   .\�5      �� $ 00:00:02��6'   $	 360Q�6:   5      5      5       � Triggers 5      :� 5   
    :� RoutedEvent$ FrameworkElement.Loaded��6   .[�5      �� #��[  6   5      .��5#      � .+�5%   
   A� -   5         A�Name$ ellipse��5%      $��55,49,54,38q�6#   $��20��68    J� RenderTransformOrigin$ 0.5,0.5&�6M   z�5'      V� 5)      .~�5+      �� �� 5-      �� 5/      51      ��55      �� 57       �� EndPoint$ 0.445,0.997&�6&    �� 
StartPoint$ 0.555,0.003&�6=   .��59      �  � Color$ White��6#    � Offset$ 0Q�61   � 5;      $ 	#FF0052FF��6#   $ 1Q�65   5=      5A      �� 5B   
   -   5        $��textBox1��5B   M   $��33��6   $
��Left=�6   $��
88,116,0,0q�69   $	��Top=�6]   $	��108��6u   $��Loading ...��6�   $
��Bold"�6�    �� TextAlignment$ Center=�6�   5C      5E      