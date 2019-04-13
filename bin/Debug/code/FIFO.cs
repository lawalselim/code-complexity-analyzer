using System;
using System.IO;
public class FIFO 
{			
    public static void main(string[]args) throws IOException 
{
        BufferedReader br = new BufferedReader(new InputStreamReader(System.in));	
 int frames, pointer = 0, hit = 
0, fault = 0,ref_len;
      int buffer[];
	        int reference;
        int mem_layout[][];		
        Console.WriteLine("Please enter the number of Frames: ");
	        frames = Integer.parseInt(br.readLine());			
        Console.WriteLine("Please enter the length of the Reference string: ");
        ref_len = Integer.parseInt(br.readLine());			
        reference = new int[ref_len];
	        mem_layout = new int[ref_len][frames];	
        buffer = new int[frames];
	        for(int j = 0; j < frames; j++)	
                buffer[j] = -1;			
        Console.WriteLine("Please enter the reference string: ");
	        for(int i = 0; i < ref_len; i++)
        {	
   reference[i] = Integer.parseInt(br.readLine());	
        }	
        Console.WriteLine();
        for(int i = 0; i < ref_len; i++)
        {
	         int search = -1;
         for(int j = 0; j < frames; j++)	  
      {
          if(buffer[j] == reference[i])
          {
	           search = j;
           hit++;
           break;
          } 
	         }	
         if(search == -1)
         {
          buffer[pointer] = 
reference[i];
	          fault++;
          pointer++;
	          if(pointer == frames)
           pointer = 0;
         }	
            for(int j = 0; j < frames; j++)
	                mem_layout[i][j] = buffer[j];
        }       			
 for(int i = 0; i < frames; i++)
	        {	
            for(int j = 0; j < ref_len; j++)	
                Console.Write("%3d ",mem_layout[j][i]);
  Console.WriteLine();	
        }		
  Console.WriteLine("The number of Hits: " + hit);
        Console.WriteLine("Hit Ratio: " + (float)((float)hit/ref_len));
   Console.WriteLine("The number of Faults: " + fault);
	    }		
}