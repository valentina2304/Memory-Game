   M S B A M L   `   `   `  ����E  AMemoryGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:MemoryGame.ViewModelsMemoryGame.ViewModels  5   	   =#clr-namespace:MemoryGame.ConvertersMemoryGame.Converters  5   	       MemoryGame.Views.GameView   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2vm#clr-namespace:MemoryGame.ViewModels     5   	   :
converters#clr-namespace:MemoryGame.Converters     5   	     � Title$  
Game Setup��5	   	   $	��600��6   $	��650��6)    � WindowStartupLocation$ CenterScreen=�5
   	   ���	#FFD4C3F4 � 	Resources 5         BoolToVisibilityConverter<   6MemoryGame.Converters.InverseBoolToVisibilityConverter $  InverseBoolToVisibilityConverter��%:   &	        &	      �� 5   
   6'     5   
   66   5      .��5      � $��20q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   o� $��0��6   $��	0,0,30,30q�6   
�������$��Right=�6J   .@�5      m� $��
Statistics��6   ���	#FF8867C6 m� Command 6+   �� *ShowStatisticsCommand+6U   m� 5      $
��Help��6   ���	#FF8867C6.@�5      m� $��About��6   
����������	#FF8867C6 6*   �� *ShowAboutCommand+6j   5      5      �� 5!   
   $��1��6   
����������	#FF8867C6$��1q�6Q    �� CornerRadius$ 5��6e    �� Padding$ 15q�6v   $��0,0,0,15q�6�   ._�5"      � $��Right=�6    � ColumnDefinitions 5#      �� 5$      $
��Auto �6'   �� 5%      $��* �6'   .+�5'      �� $��0��6   $��80��6(   $��80��63   $��Uniform=�6j   $��0,0,15,0q�6|   ��6?   �� *CurrentPlayer.ImagePath+6|   �� 5(      $��1��6   $��Center=�6-   .+�5)      �� $��Player: ��6    $
��Bold"�60   ���	#FF8867C6$��Center=�6Y   �� 5*      $��16(�6H   ���	#FF8867C6$��5,0,0,0q�6m   $��Center=�6~   ��6    �� *CurrentPlayer.Username+6~   5+      5,      5-      � 5/   
   $��2��6    50      �� 51      $��* �6#   �� 52      $��* �6#   .+�55      �� $��0��6   $��Options��6'   $��0,0,10,0q�68   ���	#FF8867C6���	#FF8867C6��56      �� 57       �� Color$ LavenderBlush��6&   	 �� Opacity$		 0.7Q�6<   .��59      �� $��10q�6   $��Center=�6)   .+�5:      �� 
 �� Orientation$
 
Horizontal=�6!   $��Center=�6;   .+�5;      d� $��Standard (4x4)��6'   $	��110��6e   $��	0,20,20,0q�6q   ���	#FF8867C6
����������	#FF8867C6$��50��5<   a    d� 	IsChecked 5;   @   �� *IsStandardMode+5<   a   d� 5=      $��Custom��6'   $	��110��6\   $��	0,20,20,0q�6h   ���	#FF8867C6
����������	#FF8867C6$��50��5>   a    5=   8   �� *IsCustomMode+5>   a   5?      �� 5A      $��0,10,0,0q�6!   {�63   �� *IsCustomMode+.+�5B      �� $
 
Horizontal=�6%   $��0,10,0,0q�6>   .+�5C      �� $��Board Size (MxN):��6(   ���	#FF8867C6$��Center=�6X   $��0,0,10,0q�6s   �� 5D      $��40��6g   $��0,0,5,0q�6r   ���	#FF8867C6��6&   �� *
CustomRows+ �� UpdateSourceTrigger$ PropertyChanged=�6�   �� 5E      $��x��6(   ���	#FF8867C6$��Center=�6H   $��0,0,5,0q�6c   �� 5F      $��40��6j   ���	#FF8867C6��6&   �� *CustomColumns+$ PropertyChanged=�6u   5G      �� 5I      
���  ��$
��Wrap=�6\   ��6$   �� *CardCountErrorMessage+y�5J      �� *IsValidCardCount+ �� 	Converter# [ 5K      �� 5M      $
 
Horizontal=�6!   $��0,20,0,0q�6:   .+�5N      �� $��Time per player (sec):��6$   ���	#FF8867C6$��Center=�6Y   $��0,0,10,0q�6t   �� 5O      $��80��6j   ���	#FF8867C6��6"   �� *PlayerTimeSeconds+$ PropertyChanged=�6u   5P      5Q      5R      �� 5T      $��1��6   $��Category��6'   $��10,0,0,0q�69   ���	#FF8867C6���	#FF8867C6��5U      �� 5V      $ LavenderBlush��6&   $		 0.7Q�6<   .��5X      �� $��10q�6   $
 Vertical=�6)   .+�5Y      d� $��Animals��6#   $	��159��6W   $��0,20,0,0q�6c   ���	#FF8867C6
����������	#FF8867C6$��60��5Z   ]    5Y   5   �� *CategoryOne+5Z   ]   d� 5[      $
��Food��6#   $	��160��6T   $��0,20,0,0q�6`   ���	#FF8867C6
����������	#FF8867C6$��60��5\   ]    5[   2   �� *CategoryTwo+5\   ]   d� 5]      $��Travel��6#   $	��160��6X   $��0,20,0,0q�6e   ���	#FF8867C6
����������	#FF8867C6$��60��5^   ]    5]   4   �� *CategoryThree+5^   ]   5_      5`      5a      �� 5c   
   $��3��6   $
 
Horizontal=�6"   $��Center=�6;   $��0,20,0,0q�6X   .+�5d      �� $��New Game��6   $	��120��6M   $��35��6Y   $
��10,0q�6e   
����������	#FF8867C6���	#FF8867C6��6(   �� *StartGameCommand+5e   G   �� 5f      $��	Open Game��6   $	��120��6M   $��35��6Y   $
��10,0q�6e   
����������	#FF8867C6���	#FF8867C6��6)   �� *OpenGameCommand+5g   G   �� 5h      $��Back to Login��6   $	��120��6T   $��35��6`   $
��10,0q�6l   
����������	#FF8867C6���	#FF8867C6��6-   �� *BackToLoginCommand+5i   G   5j      5k      5l      