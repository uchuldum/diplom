   M S B A M L   `   `   `  ���� H  Dpisateli_tuvy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:pisateli_tuvypisateli_tuvy  -clr-namespace:pisateli_tuvypisateli_tuvy      pisateli_tuvy.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     -localclr-namespace:pisateli_tuvy     -shemaclr-namespace:pisateli_tuvy     -     �Name$  main_window�� � Title$ Писатели Тувы��$��Gulim)� � 	Resources    brushWatermarkBackground �� Color  brushWatermarkBorder   BooleanToVisibilityConverter  pisateli_tuvy.Helper 
 Helper  EntryFieldStyle �� 
TargetType �� Property �� Value��%�   &	        &	      &	 #     &	 (     &	 -     �� $ White���� $ Indigo����   �� " �.���  � ��$ Stretch=���  � ��$ Center=���  � ��$
 20,0q�.���� .����  �� Orientation$ Vertical=�$
��1330��.+��� -    ��Name$ Header��$	��120�����	#FF0096D3.+��� $ 
Horizontal=�$
��1200��$��Center=�.+��� -   	 ��Name$	 main_img��$��Center=�$
��Left=�$	��107��$	��440���� $ 
Horizontal=�$��	240 0 0 0q�$��Center=�.+��� -   
 ��Name$
 button2��$��0q�$��Чогаалчылар�����	#FF0096D3���	#FF0096D3$��53�� J� Opacity$ 1Q�$	��128��
�������$��Times New Roman)�$��14(��� -   $
 button3�����	#FF0096D3���	#FF0096D3$��0q�$��Программалар��$��Times New Roman)�$��14(�
�������$��53��$
��Left=�$ 1Q�$	��Top=�$	��128���� -   $
 button4�����	#FF0096D3���	#FF0096D3$��0q�$��Запись в файл��$��Times New Roman)�$��14(�
�������$��53��$
��Left=�$ 1Q�$	��Top=�$��96���� -   $
 button5�����	#FF0096D3���	#FF0096D3$��0q�$��Admin��$��Times New Roman)�$��14(�
�������$��53��$
��Left=�$ 1Q�$	��Top=�$��92���� $ 
Horizontal=�$��Right=�$	��Top=�.+��� -   $	 image2��$��Resources/tuv.png��$��14���� -	   $	 image3��$��Resources/rus.png��$��14��$	��Top=�$��22���� -
   $	 image4��$��Resources/brit.png��$��14��$
��Fill=�$	��Top=�$��18���� -   $��Content��$
��1170��$��Center=�$
��1200��.+��� -   $��main_tuv��$ Vertical=�$��Center=�$y�	Collapsed=�.+��� -    ��Name$ richTextBox1��$	��202��$
��Left=�$
��auto��$ 1Q�$y�Visible=�$��0q��� -   $	��Map��$	��630���� -   $ richTextBox2��$	��380��$
��Left=�$ 1Q�$y�Visible=�$
��auto��$��0q��� -   $ stack_distr��$y�	Collapsed=�$ 
Horizontal=�$
��1024��$	��800��.+��� -   $ writers��$ Vertical=�$	��600���� $��20 0 0 0q�$ Vertical=�$
��auto��$
��auto��.+��� -   $	 	distr_img���� -   $ dist_txt��$	��400��$	��600��$
��auto��$��0q��� -   $ pis_tuv��$y�	Collapsed=�$
��1024��$	��800��$
��auto��$ Vertical=�.+��� -    ��Name$ label1��$��Чогаалчылар��$��31��$
��Left=�$	��Top=�$	��145��$��Arial)�$
��Bold"�$��18(��� -   $ poisk��$
��auto��$ 
Horizontal=�.+�� $��2��.+��� $��Arial)�$
��5,23q�$��18(�$0��*Введите фамилию автора��
�������y�`�  `� 	Converter# [ .1���  �� ElementName$ txtLastName�� �� Path$ Text.IsEmpty��� $ txtLastName��$ 	IsFocused��� -   $��txtLastName��$��18(�$��Arial)�
������ $	��740�� �� 	MaxLength$ 50��$��30���� -    ��Name$ 	comboBox1��$��20q� �� 
IsEditable .  �� 
IsReadOnly .  �� Text$ Кожуун��$��27��$
��Left=�$	��Top=�$	��127��$��Arial)�.@��� -    ��Name$ distr0��$��Кожуун���� -   $ distr1��$��Тожу кожуун���� -   $ distr2��$ ��Каа-Хем кожуун���� -   $ distr3��$"��Тере-Хөл кожуун���� -   $ distr4��$��Эрзин кожуун���� -   $ distr5��$ ��Тес-Хем кожуун���� -   $ distr6��$��Таңды кожуун���� -    $ distr7��$��Кызыл кожуун���� -!   $ distr8��$��Кызыл хоорай���� -"   $ distr9��$ ��Пии-Хем кожуун���� -#   $ distr10��$"��Чеди-Хөл кожуун���� -$   $ distr11��$"��Улуг-Хем кожуун���� -%   $ distr12��$ ��Чаа-Хөл кожуун���� -&   $ distr13��$��Өвүр кожуун���� -'   $ distr14��$(��"Чөөн-Хемчик кожуун���� -(   $ distr15��$ ��Сүт-Хөл кожуун���� -)   $ distr16��$,��&Баруум-Хемчик кожуун���� -*   $ distr17��$$��Бай-Тайга кожуун���� -+   $ distr18��$*��$Мөңгүн-Тайга кожуун���� -,   $
 search��$��Дилээр��$��27��$��Center=�$	��111��$��Arial)��� --   $ writers2��$ Vertical=��� -.   $ pisatel��$y�	Collapsed=�$
��1500��$
��1200��$	��998��$ 
Horizontal=�.+��� -/   $
��Menu��$ Vertical=�$	��300��$
��auto��$��	0 0 100 0q�(  "System.Windows.Controls.WebBrowser  -0     Name$ 
webbrowser��$
��auto��$	��Top=�$	��800��$y�	Collapsed=��� -1   $��noname��$ Vertical=�$y�	Collapsed=�.+�R� -2   $��TVMenu��$��0q� ��%S   'Q�       �� " Q�.���  � �� �� .R��� $	��700��$
��Wrap=����� R� -3   $��TVMenu2��$��0q� ��%S   'Q�       �� " Q�.���  � �� �� .R��� $	��700��$
��Wrap=����� 