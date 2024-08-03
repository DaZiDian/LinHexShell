using System;

namespace LinHexShell
{
    public class HexDisplay
    {
        private byte[] bytes;
        private int selectedStart = -1;
        private int selectedEnd = -1;
        private int currentPage = 0;
        private const int PageSize = 512;
        private const int BytesPerLine = 16;

        public HexDisplay() { }

        public void LoadFile(string filePath)
        {
            bytes = System.IO.File.ReadAllBytes(filePath);
            DisplayPage();
        }

        public void SaveFile(string filePath)
        {
            System.IO.File.WriteAllBytes(filePath, bytes);
        }

        public void DisplayPage()
        {
            if (bytes == null) return;

            int start = currentPage * PageSize;
            int end = Math.Min(start + PageSize, bytes.Length);

            Console.Clear();
            Console.WriteLine("Offset | 00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F | ASCII");
            Console.WriteLine("-------+-------------------------------------------------+----------------");

            for (int i = start; i < end; i += BytesPerLine)
            {
                Console.Write($"{i:X6} | ");
                for (int j = 0; j < BytesPerLine; j++)
                {
                    if (i + j < end)
                    {
                        Console.Write($"{bytes[i + j]:X2} ");
                    }
                    else
                    {
                        Console.Write("   ");
                    }
                }
                Console.Write("| ");
                for (int j = 0; j < BytesPerLine; j++)
                {
                    if (i + j < end)
                    {
                        char c = (char)bytes[i + j];
                        Console.Write(char.IsControl(c) ? '.' : c);
                    }
                    else
                    {
                        Console.Write(' ');
                    }
                }
                Console.WriteLine();
            }

            // Display page navigation
            int totalPages = (int)Math.Ceiling((double)bytes.Length / PageSize);
            Console.WriteLine("|--------------page <----------------- Page {0}/{1} -----------------page >--------------|",
                currentPage + 1, totalPages);
        }

        public void SelectHexRange(string startHex, string endHex)
        {
            // ȥ�����ܴ��ڵ�ǰ׺);

            Console.WriteLine($"����ѡ��Χ: {startHex} �� {endHex}");

            // ���Խ�������Ϊʮ��������
            if (int.TryParse(startHex, System.Globalization.NumberStyles.HexNumber, null, out int start) &&
                int.TryParse(endHex, System.Globalization.NumberStyles.HexNumber, null, out int end))
            {
                Console.WriteLine($"�����ɹ�: ��ʼ {start:X}, ���� {end:X}");
                SelectRange(start, end);
            }
            else
            {
                Console.WriteLine("��Ч��ʮ������ѡ��Χ.");
            }
        }



        public void SelectRange(int start, int end)
        {
            if (start >= 0 && end < bytes.Length && start <= end)
            {
                selectedStart = start;
                selectedEnd = end;
                Console.WriteLine($"ѡ��Χ: {selectedStart:X6} �� {selectedEnd:X6}");
            }
            else
            {
                Console.WriteLine("��Ч��ѡ��Χ.");
            }
        }




        public bool HasSelection()
        {
            return selectedStart >= 0 && selectedEnd >= 0;
        }

        public byte[] GetSelectedBytes()
        {
            if (!HasSelection())
            {
                return new byte[0];
            }

            int length = selectedEnd - selectedStart + 1;
            byte[] selectedBytes = new byte[length];
            Array.Copy(bytes, selectedStart, selectedBytes, 0, length);
            return selectedBytes;
        }

        public void DisplaySelectedBytes()
        {
            if (!HasSelection()) return;

            Console.WriteLine($"ѡ�����ֽڷ�Χ�� {selectedStart:X8} �� {selectedEnd:X8}:");
            Console.WriteLine("Offset | 00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F | ASCII");

            int currentOffset = selectedStart;
            while (currentOffset <= selectedEnd && currentOffset < bytes.Length)
            {
                Console.Write($"{currentOffset:X8} | ");
                for (int i = 0; i < BytesPerLine; i++)
                {
                    if (currentOffset + i <= selectedEnd && currentOffset + i < bytes.Length)
                    {
                        Console.Write($"{bytes[currentOffset + i]:X2} ");
                    }
                    else
                    {
                        Console.Write("   ");
                    }
                }
                Console.Write("| ");
                for (int i = 0; i < BytesPerLine; i++)
                {
                    if (currentOffset + i <= selectedEnd && currentOffset + i < bytes.Length)
                    {
                        Console.Write((bytes[currentOffset + i] >= 32 && bytes[currentOffset + i] <= 126) ? $"{(char)bytes[currentOffset + i]}" : ".");
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
                Console.WriteLine();
                currentOffset += BytesPerLine;
            }
        }

        public void SetBytes(byte[] newBytes, int offset)
        {
            if (offset >= 0 && offset + newBytes.Length <= bytes.Length)
            {
                Array.Copy(newBytes, 0, bytes, offset, newBytes.Length);
            }
            else
            {
                Console.WriteLine("��Ч��ƫ���������ݳ�����Χ.");
            }
        }

        public void ReverseSelection()
        {
            if (!HasSelection()) return;

            int length = selectedEnd - selectedStart + 1;
            Array.Reverse(bytes, selectedStart, length);
        }

        public void XorSelection(byte value)
        {
            if (!HasSelection()) return;

            for (int i = selectedStart; i <= selectedEnd; i++)
            {
                bytes[i] ^= value;
            }
        }

        public void OrSelection(byte value)
        {
            if (!HasSelection()) return;

            for (int i = selectedStart; i <= selectedEnd; i++)
            {
                bytes[i] |= value;
            }
        }

        public void AndSelection(byte value)
        {
            if (!HasSelection()) return;

            for (int i = selectedStart; i <= selectedEnd; i++)
            {
                bytes[i] &= value;
            }
        }

        public void InvertSelection()
        {
            if (!HasSelection()) return;

            for (int i = selectedStart; i <= selectedEnd; i++)
            {
                bytes[i] = (byte)~bytes[i];
            }
        }

        public void Rot13Selection()
        {
            if (!HasSelection()) return;

            for (int i = selectedStart; i <= selectedEnd; i++)
            {
                bytes[i] = (byte)((bytes[i] + 13) % 256);
            }
        }

        public void RotateLeftSelection(int count)
        {
            if (!HasSelection()) return;

            int length = selectedEnd - selectedStart + 1;
            byte[] temp = new byte[length];
            Array.Copy(bytes, selectedStart, temp, 0, length);
            for (int i = 0; i < length; i++)
            {
                bytes[selectedStart + i] = temp[(i + count) % length];
            }
        }

        public byte[] GetBytes()
        {
            return bytes;
        }

        public void SetBytes(byte[] newBytes)
        {
            bytes = newBytes;
        }

        public void SelectAll()
        {
            if (bytes == null || bytes.Length == 0)
            {
                selectedStart = -1;
                selectedEnd = -1;
            }
            else
            {
                selectedStart = 0;
                selectedEnd = bytes.Length - 1;
            }
            Console.WriteLine("�����ֽ���ѡ��.");
        }

        public void Page(int pageNumber)
        {
            if (bytes == null) return;

            int startIndex = (pageNumber - 1) * PageSize;
            if (startIndex >= bytes.Length || startIndex < 0)
            {
                Console.WriteLine("ҳ�볬����Χ.");
                return;
            }

            int endIndex = Math.Min(startIndex + PageSize - 1, bytes.Length - 1);

            for (int i = startIndex; i <= endIndex; i++)
            {
                if (i % 16 == 0)
                {
                    Console.Write($"{i:X4} ");
                }
                Console.Write($"{bytes[i]:X2} ");
                if ((i + 1) % 16 == 0)
                {
                    Console.Write(" | ");
                    for (int j = i - 15; j <= i; j++)
                    {
                        Console.Write(bytes[j] >= 32 && bytes[j] <= 126 ? (char)bytes[j] : '.');
                    }
                    Console.WriteLine();
                }
            }
            Console.WriteLine();
        }

        public void PreviousPage()
        {
            if (currentPage > 0)
            {
                currentPage--;
                DisplayPage();
            }
            else
            {
                Console.WriteLine("�Ѿ��ǵ�һҳ.");
            }
        }

        public void NextPage()
        {
            int totalPages = (int)Math.Ceiling((double)bytes.Length / PageSize);
            if (currentPage < totalPages - 1)
            {
                currentPage++;
                DisplayPage();
            }
            else
            {
                Console.WriteLine("�Ѿ������һҳ.");
            }
        }

        public void FirstPage()
        {
            currentPage = 0;
            DisplayPage();
        }

        public void LastPage()
        {
            int totalPages = (int)Math.Ceiling((double)bytes.Length / PageSize);
            currentPage = totalPages - 1;
            DisplayPage();
        }
    }
}
