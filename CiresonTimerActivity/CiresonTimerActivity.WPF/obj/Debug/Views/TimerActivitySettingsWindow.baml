   M S B A M L   `   `   `  ����~  zMicrosoft.EnterpriseManagement.UI.WpfWizardFramework, Version=7.0.5000.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35��clr-namespace:Microsoft.EnterpriseManagement.UI.WpfWizardFramework;assembly=Microsoft.EnterpriseManagement.UI.WpfWizardFramework4Microsoft.EnterpriseManagement.UI.WpfWizardFramework  5      v rMicrosoft.EnterpriseManagement.UI.SMControls, Version=7.0.5000.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35�qclr-namespace:Microsoft.EnterpriseManagement.UI.WpfControls;assembly=Microsoft.EnterpriseManagement.UI.SmControls-Microsoft.EnterpriseManagement.UI.WpfControls 5       Cireson.Timer.Activity.WPF<   6Cireson.Timer.Activity.WPF.TimerActivitySettingsWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35T PWPFToolkit, Version=7.0.5000.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      �wpfwiz�clr-namespace:Microsoft.EnterpriseManagement.UI.WpfWizardFramework;assembly=Microsoft.EnterpriseManagement.UI.WpfWizardFramework     5      �
smcontrolsqclr-namespace:Microsoft.EnterpriseManagement.UI.WpfControls;assembly=Microsoft.EnterpriseManagement.UI.SmControls     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     6J   y uMicrosoft.EnterpriseManagement.UI.SdkDataAccess, Version=7.0.5000.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35v	 rMicrosoft.EnterpriseManagement.UI.FormsInfra, Version=7.0.5000.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35t
 pMicrosoft.EnterpriseManagement.UI.Controls, Version=7.0.5000.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35JCustom7http://schemas.microsoft.com/SystemCenter/Common/UI/Wpf  	 
  5      $	��400��6e   $	��209��6q   .��5	      � -   5          �Name$  maingrid��5	      
�������.+�5   
   �� .+�5      � -   5        $��	TitleGrid��5      $��0��6$   $
��Auto��61   $��10,0,0,0q�6>   .+�5      �� -   5        $��imageActivityIcon��5      $Q��K/Cireson.Timer.Activity.WPF;component/Icons/CiresonMeatball_trans_64x64.png��61   $
��None=�6�   $
��Auto��6�   $
��Left=�6�   �� 5      $%��Cireson Timer Activity Settings��6   $��20,0,0,0q�6B   $
��Auto��6T   $��Stretch=�6a   $��48��6}   $
��Bold"�6�   $��18(�6�   5      � 5      -   5        $��ConfigurationGrid��5      $��1��6,   $��	10,0,10,0q�69    � ColumnDefinitions 5      �� 5      $	��20* �6'   �� 5      $	��23* �6'    � RowDefinitions 5      �� 5      �� 5      $��0* �6$   .+�5      �� -   5        $��scrollViewer��5      ��. $
��Auto=�6K   $
��Auto��6n   $
��Auto��6{   $��Stretch=�6�   $��0,0,0,0q�6�   $��2��6�   $��2��6�   .��5      �� -   5        $��
stackPanel��5   !    �� Orientation$ Vertical=�63   $	��100��6J   $
��Auto��6W   .+�5      �� -   5        $��chkEnableLogging��5   5   $��20,0,0,0q�6#   $��Enable Logging��6M   ��. $��10��6w    d� 	IsChecked 6�   ��  �� Path$ 	LogEnable��� 5      -   5         ��Name$ datafilepathLabel_Copy1��5   K   $��20��6    $��	20,11,0,0q�6,   $��0q�6?   $��Log file path:��6l   ��.  6�   �� 5      -	   5        $��LogPathTextBlock��5   A   $��25��6"   $��	20,0,20,0q�6.   ��. $
��Auto��6j   $��20��6w   .��6�   ��5      �� 5   "   $ LogPath�6*    �� Mode$ TwoWay=�69    �� UpdateSourceTrigger$ PropertyChanged=�6G   	 �� FallbackValue$	  ��6m   .��5    ,   5!      5"      5#      5$      5%      ]� 5'   
   $��0 0 0 0q�6   $��Bottom=�6%   $
��30px��6@   $
��Auto��6N   .+�5(      �� -
   5        
 ��Name$
 	btnCancel��5(      $
��60px��6(   $��Right=�65   $��Right=�6Q   $��5 0 5 5q�6h   ��6y   �� *CancelCommand+.��6�   Cancel6�   �� 5)      -   5        $
 btnOK��5)      $
��60px��6%   $��Right=�62   $��Right=�6N   $��5 0 5 5q�6e   ��6v   �� *	OKCommand+.��6�   OK6�   5*      5+      5,      