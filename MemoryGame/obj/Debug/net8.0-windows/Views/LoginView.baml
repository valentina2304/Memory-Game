   M S B A M L   `   `   `  ����E  AMemoryGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:MemoryGame.ViewModelsMemoryGame.ViewModels  5   	        MemoryGame.Views.LoginView   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2vm#clr-namespace:MemoryGame.ViewModels     5   	     � Title$  Sign In��5   	   $	��400��6   $	��500��6&    � WindowStartupLocation$ CenterScreen=�5	   	   ���	#FFD4C3F4 � 	Resources 5      *   $MemoryGame.ViewModels.LoginViewModel   LoginViewModel  BoolToVisibilityConverter��%:   &	        &	        5   
   6   �� 5   
   6'   5      .��5      �  � DataContext# [  6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��Welcome to Memory Game!��6   $��28(�5      $
��Bold"�5       �� TextAlignment$ Center=�5      ���	#FF8867C6 J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5         ShadowDepth$ 2Q�6#      Color$ #CCCCCC��63   	   Opacity$		 0.8Q�6C   
   
BlurRadius$
 3Q�6Q   5!      � 5#   
   $��1��6   $��20q�6    � ColumnDefinitions 5$      �� 5%      $��* �6#   �� 5&      $
��Auto �6#   �� 5'      $��* �6#   .+�5*      �� $��0��6    �� DisplayMemberPath$ Username��5-      $	��220��5.      $��Center=�5/      
����������	#FF8867C6��5+      �� *Users+ �� SelectedItem 5,      �� *SelectedUser+50   1   �� 52      $��2��6   $	��150��54      $	��150��55      $��Uniform=�56      $��5q�57      ��53      �� *SelectedUser.ImagePath+57      58      �� 5:   
   $��2��6    �� Orientation$ 
Horizontal=�5;      $��Center=�5<      $��0,0,0,20q�5=      .+�5>      �� $��New User��6   ���	#FF8867C6$	��100��5A      $��5q�5B      
���������5@      �� *NewUserCommand+5C      �� 5D      $��Delete User��6   ���	#FF8867C6$	��100��5G      $��5q�5H      
���������5F      �� *DeleteUserCommand+{�5I      �� *IsUserSelected+5J      �� 5K      $
��Play��6   ���	#FF8867C6$	��100��5N      $��5q�5O      
���������5M      �� *PlayCommand+{�5P      �� *IsUserSelected+5Q      �� 5R      -   5        $
��Exit��5R      ���	#FF8867C6$	��100��5T      $��5q�5U      
�������5W      5X      � 5Z   
   -   5         �Name$ NewUserDialog��5Z      $��3��5[      ���	#80000000$y�	Collapsed=�5]      .+�5^      �� 
�������$	��300��5_      $	��350��5`       �� CornerRadius$ 5��5a       �� Padding$ 20q�5b      ._�5c      �  5d      �� 5e      $
��Auto �6(   �� 5f      $
��Auto �6(   �� 5g      $
��Auto �6(   �� 5h      $��* �6(   �� 5i      $
��Auto �6(   .+�5l      �� $��Create New User��6    $��0��5m       $��18(�5n       $
��Bold"�5o       ���	#FF8867C6$��0,0,0,15q�5q       �� 5s      $��1��6!   $	��0,5q�6.   .+�5t      �� $��	Username:��6$   ���	#FF8867C6$��Medium"�5v   $   $��0,0,0,5q�5w   $   �� 5x      
����������	#FF8867C6��6"   �� *NewUsername+ �� UpdateSourceTrigger$ PropertyChanged=�5z   "   5{      �� 5}      $��2��6!   $	��0,5q�6.   .+�5~      �� $��User Image:��6$   ���	#FF8867C6$��Medium"�5�   $   $��0,0,0,5q�5�   $   �� 5�      $ 
Horizontal=�6%   .+�5�      �� $��<<��6%   
����������	#FF8867C6���	#FF8867C6$��40��5�   %   $��0,0,5,0q�5�   %   ��5�   %   �� *PreviousAvatarCommand+5�   %   �� 5�      $	��160��5�   $   $	��100��5�   $   $��Uniform=�5�   $   ��5�   $   �� *SelectedImagePath+5�   $   �� 5�      $��>>��6%   ���	#FF8867C6
����������	#FF8867C6$��40��5�   %   $��5,0,0,0q�5�   %   ��5�   %   �� *NextAvatarCommand+5�   %   5�      5�      �� 5�      $��4��6!   $ 
Horizontal=�5�   !   $��Right=�5�   !   .+�5�      �� $��Create��6!   ���	#FF8867C6
����������	#FF8867C6$��80��5�   !   $��0,0,5,0q�5�   !   ��5�   !   �� *CreateUserCommand+5�   !   �� 5�      $��Cancel��6!   ���	#FF8867C6
����������	#FF8867C6$��80��5�   !   ��5�   !   �� *CancelCommand+5�   !   5�      5�      5�      5�      5�      5�      