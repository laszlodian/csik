   M S B A M L   `   `   `  ����K  Ge77.MeasureBase, Version=1.0.15.0, Culture=neutral, PublicKeyToken=nullW0clr-namespace:e77.MeasureBase.GUI.CustomControls"e77.MeasureBase.GUI.CustomControls  5   	   6    0e77.MeasureBase.GUI.DialogWindows.ProgressWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @gui0clr-namespace:e77.MeasureBase.GUI.CustomControls     5   	   -   5          �Name$
  this��5   	    � Title$ ProgressWindow��5   	   $	��200��6    $	��300��6-   .��5      �  � ColumnDefinitions 5   
   �� 5	      $��* �6    � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   3   -e77.MeasureBase.GUI.CustomControls.LevelMeter  -   5          Name$ progressBar1��5   &   $��0��6      Orientation$ 
Horizontal=�6<   $��20��6U   $��10q�6d   �� 5   
   -   5        $��tb_Text��5   !   $��1��6   ��60   ��  �� ElementName$
 this�� �� Path$
 Text��� 5   
   -   5        $��	bt_Cancel��5   #   $��2��6   $��Center=�5      $��Stretch=�5      $��10q�5       �� 	IsDefault . 	 �� IsCancel	 . .��5   .   Cancel5      5      5      