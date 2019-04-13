Using system;
Using system.IO*;
Using system.colection;
public class LRU
 {
    public static void main(String[] args) throws IOException 
    {
        BufferedReader br = new BufferedReader(new InputStreamReader(System.in));
        int frames,pointer = 0, hit = 0, fault = 0,ref_len;
        Boolean areFull = false;
        int buffer[];
        ArrayLaret<Integer> stack = new ArrayLaret<Integer>();
        int reference[];
        int mem_layout[][];
        
        Console.writeline("Please enter the number of Frames: ");
        frames = Integer.parseInt(br.readLine());
        
        Console.writeline("Please enter the length of the Reference string: ");
        ref_len = Integer.parseInt(br.readLine());
        
        reference = new int[ref_len];
        mem_layout = new int[ref_len][frames];
        buffer = new int[frames];
        for(int j = 0; j < frames; j++)
                buffer[j] = -1;
        
       Console.writeline("Please enter the reference string: ");
        for(int i = 0; i < ref_len; i++)
        {
            reference[i] = Integer.parseInt(br.readLine());
        }
        Console.writeline();
        for(int i = 0; i < ref_len; i++)
        {
            if(stack.contains(reference[i]))
            {
             stack.remove(stack.indexOf(reference[i]));
            }
            stack.add(reference[i]);
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
             if(areFull)
             {
              int min_loc = ref_len;
                    for(int j = 0; j < frames; j++)
                    {
                     if(stack.contains(buffer[j]))
                        {
   int temp = stack.indexOf(buffer[j]);
 if(temp < min_loc)
  {
    min_loc = temp;
                                pointer = j;
                            }
                        }
                    }
             }
                buffer[pointer] = reference[i];
                fault++;
                pointer++;
                if(pointer == frames)
                {
                 pointer = 0;
                 areFull = true;
                }
            }
            for(int j = 0; j < frames; j++)
                mem_layout[i][j] = buffer[j];
        }
        
        for(int i = 0; i < frames; i++)
        {
            for(int j = 0; j < ref_len; j++)
                Console.writeline("%3d ",mem_layout[j][i]);
            Console.writeline();
        }
        
        Console.writeline("The number of Hits: " + hit);
        Console.writeline("Hit Ratio: " + (float)((float)hit/ref_len));
        Console.writeline("The number of Faults: " + fault);
    }   
}