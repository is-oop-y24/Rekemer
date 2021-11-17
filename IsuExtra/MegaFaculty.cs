using System.Collections.Generic;
using System.Linq;

namespace IsuExtra
{
    public class MegaFaculty : Singletone<MegaFaculty>
    {
        private List<string> _faculties = new List<string>();
        private List<char> _letters = new List<char>();

        public List<string> Faculties
        {
            get => _faculties;
        }

        public bool IsFacultyExists(string faculty)
        {
            return _faculties.Any(t => t == faculty);
        }

        public string GetMegafaculty(char letter)
        {
            for (int i = 0; i < _letters.Count; i++)
            {
                if (_letters[i] == letter)
                {
                    return _faculties[i];
                }
            }

            return null;
        }

        public bool IsTherePrefix(char prefix)
        {
            return _letters.Any(t => t == prefix);
        }

        public void AddFaculty(List<string> faculties, List<char> letters)
        {
            for (int i = 0; i < faculties.Count; i++)
            {
                if (faculties[i] != null && letters[i] != '\0')
                {
                    if (!IsFacultyExists(faculties[i].ToLower()))
                    {
                        _faculties.Add(faculties[i].ToLower());
                        _letters.Add(letters[i]);
                    }
                }
            }
        }
    }
}