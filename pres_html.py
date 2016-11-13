import requests
# import unicodedata
# import pprint
import sys

reload(sys)
sys.setdefaultencoding('utf-8')

a = ["John_Adams",
"Thomas_Jefferson",
"James_Madison",
"James_Monroe",
"John_Quincy_Adams"
,"Andrew_Jackson"
,"Martin_Van_Buren"
,"William_Henry_Harrison"
,"John_Tyler"
,"James_K._Polk"
,"Zachary_Taylor"
,"Millard_Fillmore"
,"Franklin_Pierce"
,"James_Buchanan"
,"Abraham_Lincoln"
,"Andrew_Johnson"
,"Ulysses_S._Grant"
,"Rutherford_B._Hayes"
,"James_A._Garfield"
,"Chester_A._Arthur"
,"Grover_Cleveland"
,"Benjamin_Harrison"
,"Grover_Cleveland"
,"William_McKinley"
,"Theodore_Roosevelt"
,"William_Howard_Taft"
,"Woodrow_Wilson"
,"Warren_G._Harding"
,"Calvin_Coolidge"
,"Herbert_Hoover"
,"Franklin_Roosevelt"
,"Harry_S._Truman"
,"Dwight_D._Eisenhower"
,"John_F._Kennedy"
,"Lyndon_B._Johnson"
,"Richard_M._Nixon"
,"Gerald_Ford"
,"Jimmy_Carter"
,"Ronald_Reagan"
,"George_Bush"
,"Bill_Clinton"
,"George_W._Bush"
,"Barack_Obama"
,"Donald_Trump"]

for i in range(0, 44):
    f = open(str(i), "w")
    r = requests.get("https://en.wikipedia.org/wiki/"+str(a[i]))
    text = r.text
    f.write(text)
# re = open("president.txt", "r")
# a = []
# for line in re:
#     a.append(line)
# print a
# #reload(sys)
# #sys.setdefaultencoding('utf-8')
#
#
#
# for line in a:
#     name = line.strip('\n')
#     print name
#     f = open(str(name), "wb");
#
#     r = requests.get("https://en.wikipedia.org/wiki/"+str(name));
#
#     text = r.text;
#     #f.write(text);
