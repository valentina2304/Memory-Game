   M S B A M L   `   `   `  ����E  AMemoryGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:MemoryGame.ViewsMemoryGame.Views  5   	   =#clr-namespace:MemoryGame.ConvertersMemoryGame.Converters  5   	   $    MemoryGame.Views.GameBoardView   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:MemoryGame.Views     5   	   :
converters#clr-namespace:MemoryGame.Converters     5   	     � Title$  Memory Game��5	   	   $	��690��6   $	��850��6*    � WindowStartupLocation$ CenterScreen=�5
   	   ���	#FFD4C3F4 � 	Resources 5         BoolToVisibilityConverter2   ,MemoryGame.Converters.BoolToOpacityConverter  BoolToOpacityConverter0   *MemoryGame.Converters.IsImagePathConverter  IsImagePathConverter  CardTemplate �� RelativeSource �� AncestorType J� Opacity �� 	Converter �� ConverterParameter �� 
TargetType	 �� Triggers
 �� Binding �� Value �� Property �� Value��%#  &	        &	      &	 &     &	 9     7  7  7   7  7  �� 5   
   6'     5   
   6,     5   
   6*   �� 5   
   .R�5      �� $��2q�6   ��5      �� *DataContext.CardClickCommand+ ��  ItemsControl��5      ��  5      �� *	IsMatched+8   .��5      � $	��120��6   $	��120��6#   .+�5      �� 
����������	#FF8867C6$��2q�5      y�5      �� *	IsFlipped+8  $ Invert��._�5      �� $	��♣��6$   $��36(�5      ���	#FF8867C6$��Center=�5      $��Center=�5       5!      �� 5#      
����������	#FF8867C6$��2q�5%      y�5&      �� *	IsFlipped+8  ._�5'      � .+�5(      �� $��UniformToFill=�5)      $��5q�5*   !   ��5(   $   �� *	ImagePath+��5+   "   �� 5,   &   " ��6,   	 5-   *   �� 5.   .   
 6:   �� *	ImagePath+8  	 False6�   .a�5/   2   ��  � y�$ 	Collapsed=�6O   50   /   52   '   54      �� 56      $��36(�57       ���	#FF8867C6$��Center=�59       $��Center=�5:       ��56   (   �� *Value+��5;   "   �� 5<   &   " ��6,   	 5=   *   �� 5>   .   
 6:   �� *	ImagePath+8   True6�   .a�5?   2   ��  � y�$ 	Collapsed=�6O   5@   /   5B   '   5D      5E      5F      5G      5H      5I      5K      .��5M      � $��10q�6    � RowDefinitions 5N   
   �� 5O      $
��Auto �6   �� 5P      $
��Auto �6   �� 5Q      $��* �6   �� 5R      $
��Auto �6   .+�5U   
   �� $��1��6   
����������	#FF8867C6$��1q�5X       �� CornerRadius$ 5��5Y       �� Padding$ 10q�5Z      $	��0,5q�5[      ._�5\      �  � ColumnDefinitions 5]      �� 5^      $
��Auto �6'   �� 5_      $��* �6'   �� 5`      $
��Auto �6'   .+�5c      �� $��0��6    �� Orientation$ 
Horizontal=�6-   .+�5d      �� $��Player: ��6    ���	#FF8867C6$
��Bold"�6G   $��Center=�6Y   �� 5e      ���	#FF8867C6$��Center=�6_   $��0,0,15,0q�6z   ��6    �� *CurrentPlayer.Username+6z   5f      �� 5h      $��2��6   $ 
Horizontal=�6-   .+�5i      �� $��Time: ��6    ���	#FF8867C6$
��Bold"�6E   $��Center=�6W   �� 5j      ���	#FF8867C6$��Center=�6T   $��Consolas)�6o   ��6    �� *TimeDisplay+6o   5k      5l      5o      �� 5q   
   $��2��6   #��[ 5s      $
��0,10q�5t      ��5r      �� *Cards+��5u      �� 5v      .R�5w      A�  A� Rows 6"   �� *Rows+ A� Columns 68   �� *	Columns+5x      5z      �� 5|   
   $��3��6   
����������	#FF8867C6$��1q�5   	   $ 5��5�   	   $ 10q�5�   	   $��0,10,0,0q�5�   	   ._�5�      �  5�      �� 5�      $��* �6'   �� 5�      $
��Auto �6'   �� 5�      $
��Auto �6'   .+�5�      �� $��1��6   $
��Save��5�      $��0,0,10,0q�5�      $
��10,5q�5�      ���	#FF8867C6
�������$
��Bold"�5�      $	��100��5�      ��5�      �� *SaveGameCommand+5�      �� 5�      $��2��6   $
��Exit��5�      $
��10,5q�5�      ���	#FF8867C6
�������$
��Bold"�5�      $	��100��5�      ��5�      �� *ExitCommand+5�      5�      5�      5�      5�      