   M S B A M L   `   `   `  ����E  AMemoryGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:MemoryGame.ConvertersMemoryGame.Converters  5   	   %    MemoryGame.Views.OpenGameDialog   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   :
converters#clr-namespace:MemoryGame.Converters     5   	     � Title$  Open Saved Game��5   	   $	��400��6!   $	��500��6.    � WindowStartupLocation$ CenterOwner=�5   	   ���	#FFD4C3F4.��5	      � $��10q�6    � RowDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   $"��Select a saved game to open:��5      $��16(�5      $
��Bold"�5      ���	#FF8867C6$��0,0,0,10q�5      �� 5   
   -   5         ��Name$ SavedGamesList��5      $��1��5      
����������	#FF8867C6$	��0,5q�5       �� SelectionMode$ Single=�5      ��5      �� *
SavedGames+ �� SelectedItem 5      �� *SelectedGame+ �� Mode$ TwoWay=� �� View 5      �� 5       .P�5!      �� $��Player��6%    �� Width$ 90��65   	 �� DisplayMemberBinding	 6@   �� *
PlayerName+�� 5"      $
��Date��6%   $	 120��63   	 6?   �� *	SavedAt+
 �� StringFormat$
 {0:dd/MM/yyyy HH:mm}���� 5#      $
��Mode��6%   $ 60��63   	 6>   �� *
GameMode+�� 5$      $��Board��6%   $ 50��64    �� CellTemplate 5%      �� 5&      .R�5'   "   �� ��5(   &   `� 5)   *   $
 {0}x{1}��67   .1�5*   .   ��  �� Path$
 Rows�66   �� 5+   .   $ Columns�66   5,   +   5.   #   5/      51      �� 52      $��
Total Time��6%   $ 65��69   	 6D   �� *	TotalTime+$
 {0} sec���� 53      $��	Time Left��6%   $ 65��68   	 6C   �� *RemainingTime+$
 {0} sec��54      56      �� 58   
   $��2��6    �� Orientation$ 
Horizontal=�59      $��Right=�5:      $��0,10,0,0q�5;      .+�5<      �� $��Delete��6   $��80��5=      $��30��5>      $��0,0,10,0q�5?      
����������	#FF8867C6���	#FF8867C6��5C      �� *DeleteGameCommand+�� 5E      $
��Open��6   $��80��5F      $��30��5G      $��0,0,10,0q�5H      
����������	#FF8867C6���	#FF8867C6��5L      �� *OpenGameCommand+�� 5N      $��Cancel��6   $��80��5O      $��30��5P      
����������	#FF8867C6���	#FF8867C6��5T      �� *CancelCommand+5U      5V      5W      