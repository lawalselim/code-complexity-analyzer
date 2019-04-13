using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
 
namespace Huffman_Data_Comperision
{
    class huffman
    {
        private int lenghtoftext;
        //public static int arraysize = 1;
        //public int size=arraysize;
        private int lenght;
        public void sort(ArrayList st,ArrayList doub)
        {
            int array_size = doub.Count;
            int i, j;
            int min;
 
            for (i = 0; i < array_size - 1; i++)
            {
                min = i;
                for (j = i + 1; j < array_size; j++)
                {
                    if ((double)doub[j] > (double)doub[min])
                        min = j;
                }
                double temp1 = (double)doub[i];
                doub[i] = doub[min];
                doub[min] = temp1;
 
                string temp2 = (string)st[i];
                st[i] = st[min];
                st[min] = temp2;
 
            }
        }
 
        public bool FindEntry(string entry,ArrayList arr)
        {
            for (int i = 0; i < arr.Count; i++)
            {
                if (arr[i].Equals(entry))
                    return true;
            }
            return false;
        }
 
        public void pass(ArrayList st,ArrayList st_big)
        {
            for (int i = 0; i < st.Count; i++)
            {
                st_big.Add(st[i]);
               // doub_big.Add(doub[i]);
            }
        }
 
        public void setlenght(int l)
        {
            lenght = l;
        }
        public int getlenght()
        {
            return lenght;
        }
        public ArrayList dic_character = new ArrayList();
        public ArrayList dic_code = new ArrayList();
        public void set_dic_character(string arr)
        {
           // for (int i = 0; i < arr.Length; i++)
            //{
                dic_character.Add(arr);
            //}
        }
        public void set_dic_code(string arr)
        {
            //for (int i = 0; i < arr.Length; i++)
            //{
                dic_code.Add(arr);
            //}
        }
    }
}
 
/*public  calculateprob [] array=new calculateprob [arraysize]; 
public void search(string var)
{
     for (int i = 0; i < size; i++)
     {
         if (array[i].variable==var)
         {
             array[i].setcount(array[i].getcount() + 1);
             array[i].probability = array[i].getcount()/getlenght();
            // return true;
             break;
         }
         else
         {
             array[i].variable = var;
             array[i].setcount(array[i].getcount() + 1);
             array[i].probability = array[i].getcount()/getlenght();
             arraysize++;
                
         }
     }
 }*/
/*public struct calculateprob
{
    public string variable;
    public double probability;
    public int count;
    public void intilizecount()
    {
        count = 0;
    }
    public void setcount(int c)
    {
        count=c;
    }
    public int getcount()
    {
        return count;
    }
}
*/