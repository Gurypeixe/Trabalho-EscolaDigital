using System.IO;
using System;
using Guryflix.Estruturas;

namespace Guryflix.Utilitarios
{
    class UtilitariosFicheiros
    {
        private int numberOfFiles;
        public int numberOfLines;
        public string content;
        public string[] fileDirectories;

        public UtilitariosFicheiros()
        {
            numberOfFiles = 0;
            numberOfLines = 0;
            content = "";
        }

        public ListaLigada ImportarParaListaLigada(string fileDirectory)
        {
            ListaLigada obj = new ListaLigada();
            numberOfLines = 0;
            CriarFicheiro(fileDirectory);
            FileStream fs = new FileStream(fileDirectory, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            sr.BaseStream.Seek(0, SeekOrigin.Begin);
            string str = "NULL";
            while (str != null)
            {
                str = sr.ReadLine();
                numberOfLines++;
                obj.InserirInicio(str);
            }
            sr.Close();
            fs.Close();
            return obj;
        }

        public Pilha ImportarParaPilha(string fileDirectory)
        {
            Pilha obj = new Pilha();
            numberOfLines = 0;
            FileStream fs = new FileStream(fileDirectory, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            sr.BaseStream.Seek(0, SeekOrigin.Begin);
            string str = "NULL";
            while (str != null)
            {
                str = sr.ReadLine();
                numberOfLines++;
                obj.Empilhar(str);
            }
            sr.Close();
            fs.Close();
            return obj;
        }

        public string[] RetornarConteudo(string fileDirectory)
        {
            Fila queue = new Fila();
            FileStream fs = new FileStream(fileDirectory, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            sr.BaseStream.Seek(0, SeekOrigin.Begin);
            string str = "NULL";
            while (str != null)
            {
                str = sr.ReadLine();
                queue.Enfileirar(str);
            }
            sr.Close();
            fs.Close();
            string[] files = new string[(queue.fim)];
            for (int i = 0; i < (queue.fim); i++)
            {
                files[i] = queue.Desenfileirar();
            }
            return files;
        }

        public string[] LerTodosFicheiros(string fileDirectory)
        {
            Fila queue = new Fila();
            string[] paths = { };
            paths = Directory.GetFiles(fileDirectory);
            foreach (String path in paths)
            {
                string imageName = Path.GetFileName(path);
                string[] fileName = imageName.Split('.');
                queue.Enfileirar(fileName[0]);
            }
            string[] files = new string[(queue.fim + 1)];
            for (int i = 0; i < (queue.fim + 1); i++)
                files[i] = queue.Desenfileirar();
            return files;
        }

        public int CalcularTamanhoFicheiro(string fileDirectory)
        {
            int filesSize = 0;
            FileStream fs = new FileStream(fileDirectory, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            sr.BaseStream.Seek(0, SeekOrigin.Begin);
            string str = sr.ReadLine();
            while (str != null)
            {
                str = sr.ReadLine();
                filesSize++;
            }
            sr.Close();
            fs.Close();
            return filesSize;
        }

        public void LerDados(string fileDirectory)
        {
            numberOfLines = 0;
            FileStream fs = new FileStream(fileDirectory, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            sr.BaseStream.Seek(0, SeekOrigin.Begin);
            string str = sr.ReadLine();
            while (str != null)
            {
                str = sr.ReadLine();
                numberOfLines++;
                content = str;
            }
            sr.Close();
            fs.Close();
        }

        public void EscreverDados(string fileDirectory, string data)
        {
            FileStream fs = new FileStream(fileDirectory, FileMode.Append, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine((data));
            sw.Flush();
            sw.Close();
            fs.Close();
        }

        private void GuardarDiretoriosDeFicheiro(string srcFileDirectory, string destFileDirectory)
        {
            LerDados(srcFileDirectory);
            fileDirectories = new string[numberOfLines];
            FileStream fs = new FileStream(srcFileDirectory, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            sr.BaseStream.Seek(0, SeekOrigin.Begin);
            string str = sr.ReadLine();
            string defaultFileDirectory = "";
            numberOfFiles = 0;
            defaultFileDirectory = (destFileDirectory + str);
            fileDirectories[numberOfFiles] = defaultFileDirectory;
            
            while (str != null)
            {
                if (numberOfFiles == numberOfLines - 1)
                    break;
                numberOfFiles++;
                str = sr.ReadLine();
                defaultFileDirectory = (destFileDirectory + str);
                fileDirectories[numberOfFiles] = defaultFileDirectory;
            }
            sr.Close();
            fs.Close();
            numberOfLines = 0;
        }

        public int CalcularTamanhoDiretorios(string srcFileDirectory, string destFileDirectory, string extension)
        {
            GuardarDiretoriosDeFicheiro(srcFileDirectory, destFileDirectory);
            int filesSize = 0;
            for (int i = 0; i <= numberOfFiles; i++)
            {
                FileStream fs = new FileStream((fileDirectories[i] + extension), FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);
                sr.BaseStream.Seek(0, SeekOrigin.Begin);
                string str = sr.ReadLine();
                while (str != null)
                {
                    str = sr.ReadLine();
                    filesSize++;
                }
                sr.Close();
                fs.Close();
            }
            numberOfLines = filesSize;
            return filesSize;
        }

        public string[] RetornarConteudoDiretorios(string srcFileDirectory, string destFileDirectory, string extension, int sizeOfArr, int numberOfFiles)
        {
            if (numberOfFiles == -1)
                numberOfFiles = CalcularTamanhoDiretorios(srcFileDirectory, destFileDirectory, extension);
            int j = 0;
            string[] arr = new string[sizeOfArr];
            for (int i = 0; i < numberOfFiles; i++)
            {
                FileStream fs = new FileStream((fileDirectories[i] + extension), FileMode.Open, FileAccess.Read);
                Console.WriteLine(fileDirectories[i]);
                StreamReader sr = new StreamReader(fs);
                sr.BaseStream.Seek(0, SeekOrigin.Begin);
                string str = "null";
                while (str != null)
                {
                    if (j == sizeOfArr)
                    {
                        break;
                    }
                    if ((str[0] >= 65 && str[0] <= 90) || (str[0] >= 97 && str[0] <= 122))
                    {
                        str = sr.ReadLine();
                        if (str != null)
                        {
                            arr[j] = str;
                            j++;
                        }
                    }
                }
                sr.Close();
                fs.Close();
            }
            return arr;
        }

        public void CriarFicheiro(string path)
        {
            if (!File.Exists(path))
            {
                FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write);
                fs.Close();
            }
        }

        public void CriarDiretorio(string fileDirectory)
        {
            if (!Directory.Exists(fileDirectory))
                Directory.CreateDirectory(fileDirectory);
        }
    }
}
