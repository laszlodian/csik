   M S B A M L   `   `   `  ����K  Ge77.MeasureBase, Version=1.0.15.0, Culture=neutral, PublicKeyToken=nullW0clr-namespace:e77.MeasureBase.GUI.CustomControls"e77.MeasureBase.GUI.CustomControls  5      3    -e77.MeasureBase.GUI.CustomControls.LevelMeter   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      @gui0clr-namespace:e77.MeasureBase.GUI.CustomControls     5      -   5        $��myLevelMeter��5      $��10��6   $��10��6'     � 	Resources  5	      ��%    5      .��5      � $��Stretch=�6   $��Stretch=�6'    � DataContext 6E   ��  �� ElementName$ myLevelMeter��.+�5   
   �� $	��1pxq�6   
���   ���5      �� 5       �� 
StartPoint$	 0,0&�6&    �� EndPoint$	 0,1&�67   .��5      �  � Color$ #C0CBDF��6#    � Offset$ 0Q�63   � 5      $ #DCE9FA��6#   $ 1Q�63   5      5      �� 5   
   $
��Left=�6   $��1,1,1,1q�60    �� Orientation$ 
Horizontal=�6A   .+�5      � -   5        $��HorizontalProgressBar��5      $
��Left=�60   ��5      `� 5       `� 	Converter 5      @   :e77.MeasureBase.GUI.CustomControls.VerticalHeightConverter  5      6<   .1�5       �� 	 �� Path$	 Value�6"   
 �� Mode$
 OneWay=�6/   6?   �� 5!      $	 ActualWidth�6"   $
 OneWay=�65   6E   5"      y�5$      �� 5%      $	 Orientation�6   $
 OneWay=�61    �� ConverterParameter$ 
Horizontal��6?    �� 	Converter 5&      I   Ce77.MeasureBase.GUI.CustomControls.OrientationToVisibilityConverter  5'      5)      .+�5+      �� $��1q�6   
���   ���5-      `� 5.       5/      7   1e77.MeasureBase.GUI.CustomControls.BrushConverter  50   "   .1�52      �� $	 Value�6&   $
 OneWay=�69   �� 53      $	 	BaseColor�6&   $
 OneWay=�69   �� 54      $	 BoundingColors�6&   $
 OneWay=�6<   55      57      �� 59      ��5;      �� 5<      $	 0,0&�6.   $
  0,1&�6?   .��5>      � $ 	#7F000000��6+   $ 0Q�6?   � 5?      $ Transparent��6+   $	 0.5Q�6?   � 5@      $ 	#3FFFFFFF��6+   $	 0.5Q�6?   � 5A      $ 	#7FFFFFFF��6+   $
 0.75Q�6?   � 5B      $ Transparent��6+   $ 1Q�6?   5C      5E      5F      � 5G      -   5        $$��secondaryHorizontalProgressBar��5G       J� RenderTransformOrigin$	 0,1&�69   $
��Left=�6U   ��5H      `� 5I       5J        5K      6<   .1�5M      �� $	 SecondaryValue�6"   $
 OneWay=�68   6H   �� 5N      $	 ActualWidth�6"   $
 OneWay=�65   6E   5O      y�5Q      �� 5R      $	 Orientation�6   $
 OneWay=�61   $ 
Horizontal��6?    5S        5T      5V      .+�5X      �� $��1q�6   
���   ���5Z      �� 5[       �� Color 5\      �� 5b      $	 SecondaryBaseColor�6&   $
 OneWay=�6B   5f      5h      �� 5i      ��5k      �� 5l      $	 0,0&�6.   $
  0,1&�6?   .��5m      � $ 	#7F000000��6+   $ 0Q�6?   � 5n      $ Transparent��6+   $	 0.5Q�6?   � 5p      $ 	#7FFFFFFF��6+   $
 0.75Q�6?   � 5q      $ Transparent��6+   $ 1Q�6?   5r      5t      5u      5v      �� 5x   
   $��Bottom=�6   $��1,1,1,1q�60   .+�5y      � -   5        $"��secondaryVerticalProgressBar��5y      $	 0,1&�67   $��Bottom=�6S   ��5z      `� 5{       5|        5}      .1�5      �� $	 SecondaryValue�6"   $
 OneWay=�68   6H   �� 5�      $	 ActualHeight�6"   $
 OneWay=�66   6F   5�      y�5�      �� 5�      $	 Orientation�6   $
 OneWay=�61   $ Vertical��6?    5�        5�      5�      .+�5�      �� $��1q�6   
���   ���5�      �� 5�       5�      �� 5�      $	 SecondaryBaseColor�6&   $
 OneWay=�6B   5�      5�      �� 5�      ��5�      �� 5�      $	 0,0&�6.   $
  1,0&�6?   .��5�      � $ 	#7F000000��6+   $ 0Q�6?   � 5�      $ Transparent��6+   $	 0.5Q�6?   � 5�      $ 	#7FFFFFFF��6+   $
 0.75Q�6?   � 5�      $ Transparent��6+   $ 1Q�6?   5�      5�      5�      � 5�      -   5        $��VerticalProgressBar��5�      $	 0,1&�6.   $��Bottom=�6J   ��5�      `� 5�       5�        5�      6<   .1�5�      �� $	 Value�6"   $
 OneWay=�6/   6?   �� 5�      $	 ActualHeight�6"   $
 OneWay=�66   6F   5�      y�5�      �� 5�      $	 Orientation�6   $
 OneWay=�61   $ Vertical��6?    5�        5�      5�      .+�5�      �� $��1q�6   
���   ���5�      `� 5�       5�        5�   "   .1�5�      �� $	 Value�6&   $
 OneWay=�69   �� 5�      $	 	BaseColor�6&   $
 OneWay=�69   �� 5�      $	 BoundingColors�6&   $
 OneWay=�6<   5�      5�      �� 5�      ��5�      �� 5�      $	 0,0&�6.   $
  1,0&�6?   .��5�      � $ 	#7F000000��6+   $ 0Q�6?   � 5�      $ Transparent��6+   $	 0.5Q�6?   � 5�      $ 	#3FFFFFFF��6+   $	 0.5Q�6?   � 5�      $ 	#7FFFFFFF��6+   $
 0.75Q�6?   � 5�      $ Transparent��6+   $ 1Q�6?   5�      5�      5�      5�      (� 5�   
   $��3,3,3,3q�6   $��30��6#   y�5�      �� 5�      $	 TextPercentageVisibility�6   $
 OneWay=�6:   ��5�      �� 5�      $	 ActualWidth�6   $
 OneWay=�6-    5�      ;   5e77.MeasureBase.GUI.CustomControls.TextWidthConverter  5�      63   5�      .��5�      �� $��Center=�6   
�������$��Stretch=�6H   ��5�      �� 5�      $	 Value�6"   $
 TwoWay=�6/    5�      D   >e77.MeasureBase.GUI.CustomControls.PercentStringValueConverter  5�   "   6D   5�      5�      5�      5�      5�      