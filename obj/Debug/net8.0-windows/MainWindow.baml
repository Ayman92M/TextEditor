   M S B A M L   `   `   `  ����E  ATextEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:TextEditor
TextEditor  5   	       TextEditor.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:TextEditor     5   	   -   5          � Title$  Text Editor��5   	   $	��450��6   $	��800��6*    J� 	AllowDrop$
 True��5	   	    � Icon$# my_icon/texteditor_pencil.ico��5
   	    J� CommandBindings 5      )  #System.Windows.Input.CommandBinding  5   
   -   5           Command$	 New��5      6(     5   
   -   5        $
 Open��5      6(     5   
   -   5        $
 Save��5      6(     5   
   -   5        $  ApplicationCommands.SaveAs��5      6>     5   
   -   5        $ ApplicationCommands.Close��5      6>     5   
   -   5        $ Close��5      6>     5   
   -   5        $ ApplicationCommands.Copy��5      6=     5   
   -	   5        $ ApplicationCommands.Cut��5      6=     5   
   -
   5        $ ApplicationCommands.Paste��5      6=     5   
   -   5        $ ApplicationCommands.Help��5      6=    � 	Resources 5         	MenuStyle �� 
TargetType �� Property �� Value��%�   &	        �� 5   
   " ��6"   .�5      ��  � ��
 �����6,   �� 5        � ��$ 0,0,1,0q�60   �� 5!       � ��$ 4,0,12,0q�6(   5"      5#      .��5&      � 	 � RowDefinitions	 5(   
   �� 5)      $
��auto �6   6,   �� 5*      $��* �6   6)   �� 5+      $
��auto �6   6,   .+�5/   
   � $��0��6   .+�51      o� 
�������$��0,1,0,0q�6*   .@�53      m� -   5        $��Me��53      $
��File��6&   .@�54      m� $
��_New��6    
 m� Command$	
 New��6/   $
{�True��6>    m� Icon 55      �� 56      $��my_icon/newfile.png��6$   58      m� 5;      $��_Open��6    $

 Open��6/    5<      �� 5=      $��my_icon/open.ico��6$   5?      m� 5B      $
��Save��6    $

 Save��6/    5C      �� 5D      $��my_icon/floppy_save.ico��6$   5F      m� 5I      $��Save As��6    $ 
 ApplicationCommands.SaveAs��61    m� InputGestureText$ Ctrl+Shift+S��5J   \    5L      �� 5M      $��my_icon/save_as.ico��6$   5P      �� 5R      m� 5S      $
��Exit��6   $ Alt+F4��6-   $
 ApplicationCommands.Close��6G    5T      �� 5U      $��my_icon/exit.png��6$   5W      5X      m� 5Z      $
��Edit��6   .@�5\      m� $
 ApplicationCommands.Cut��6    5]      �� 5^      $��my_icon/cut.ico��6$   5`      m� 5b      $
 ApplicationCommands.Copy��6    5c      �� 5d      $��my_icon/copy.ico��6$   5f      m� 5h      $
 ApplicationCommands.Paste��6    5i      �� 5j      $��my_icon/paste.ico��6$   5l      5n      m� 5p      $
��Help��6   .@�5q      m� $��About��6   $
 ApplicationCommands.Help��6.    5r      �� 5s      $��my_icon/info.ico��6$   5u      5v      5w      5x      � 5{   
   $��1��6   $
 True��6   .+�5|      �� -   5        $��	MyTextBox��5|   #   $��1��6    �� TextWrapping$
 Wrap=�64   " �� VerticalScrollBarVisibility$
 Auto=�6I    �� AcceptsReturn$
 True��5}   8   $
 True��5~      6B   5      � 5�   
   $��2��6   .+�5�      �� -   5        $��	statusBar��5�      .@�5�      �� -   5        $��Status��5�       $��Status: Ready.��6.   �� 5�      #��[  6    .��5�      ��  �� Orientation$ 
Horizontal=�6!   .+�5�      �� -   5        $��
StatusIcon��5�       $��my_icon/Check2.png��63   $��16��6O   $��16��6Z   5�      5�      �� 5�      -   5        $"��TeckenCountIncludeWhitespace��5�       $!��0 Tecken include whitespace��6E   #��[  6k   �� 5�      -   5        $"��CharCountExcludingWhitespace��5�       $#��0 Tecken excluding whitespace��6E   #��[  6m   �� 5�      -   5        $��	WordCount��5�       $��0 Words��63   #��[  6E   �� 5�      -   5        $��	LineCount��5�       $��0 Line��63   #��[  6E   5�      5�      5�      5�      