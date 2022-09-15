// // See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");

using System.Security.Cryptography.X509Certificates;
using System.Text;

class EnigmaMachine
{
    private const string alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";

    public static Dictionary<char, int> LetterToNumber = alphabet.Select((x, y) => (Letter: x, Ord: y+1)).ToDictionary(x => x.Letter, x => x.Ord);

    public static Dictionary<int, char> NumberToLetter = alphabet.Select((x, y) => (Letter: x, Ord: y + 1)).ToDictionary(x => x.Ord, x => x.Letter);

    private static Rotor r1, r2, r3, reflector;
    private static PlugBoard plugBoard;

    private static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        InitialiseComponents();

        var word = args[0];
        Console.WriteLine(word);

        var encodedWord = "";
        foreach (var character in word)
        {
            // Console.WriteLine(character);
            var number = LetterToNumber[character];

            if (r1.MoveRotor())
            {
                if (r2.MoveRotor())
                {
                    r3.MoveRotor();
                }
            }

            Console.WriteLine("Before plugboard: " + number);
            number = plugBoard.EncodeForward(number);
            Console.WriteLine("After plugboard: "+number);
            Console.WriteLine(" ");

            Console.WriteLine("Before r1: " + number);
            number = r1.EncodeForward(number);
            Console.WriteLine("After r1: "+number);
            Console.WriteLine(" ");

            Console.WriteLine("Before r2: " + number);
            number = r2.EncodeForward(number);
            Console.WriteLine("After r2: " + number);
            Console.WriteLine(" ");

            Console.WriteLine("Before r3: " + number);
            number = r3.EncodeForward(number);
            Console.WriteLine("After r3: " + number);
            Console.WriteLine(" ");

            Console.WriteLine("Before reflector: " + number);
            number = reflector.EncodeForward(number);
            Console.WriteLine("After reflector: " + number);
            Console.WriteLine(" ");

            Console.WriteLine("Before r3: " + number);
            number = r3.EncodeReverse(number);
            Console.WriteLine("After r3: " + number);
            Console.WriteLine(" ");

            Console.WriteLine("Before r2: " + number);
            number = r2.EncodeReverse(number);
            Console.WriteLine("After r2: " + number);
            Console.WriteLine(" ");

            Console.WriteLine("Before r1: " + number);
            number = r1.EncodeReverse(number);
            Console.WriteLine("After r1: " + number);
            Console.WriteLine(" ");

            Console.WriteLine("Before plugboard: " + number);
            number = plugBoard.EncodeReverse(number);
            Console.WriteLine("After plugboard: " + number);
            Console.WriteLine(" ");

            encodedWord += NumberToLetter[number];
            Console.WriteLine("Word so far: "+encodedWord);
            Console.WriteLine("---------------------------------");
        }

        Console.WriteLine(encodedWord);
    }

    private static void InitialiseComponents()
    {
        //Cesar +3
        r1 = new Rotor(new (int From, int To)[]
            {
                (1, 4), (2, 5), (3, 6), (4, 7), (5, 8), (6, 9), (7, 10), (8, 11), (9, 12), (10, 13), (11, 14), (12, 15),
                (13, 16), (14, 17), (15, 18), (16, 19), (17, 20), (18, 21), (19, 22), (20, 23), (21, 24), (22, 25),
                (23, 26), (24, 27), (25, 28), (26, 29), (27, 30), (28, 31), (29, 32), (30, 33), (31, 1), (32, 2),
                (33, 3)
            })
        {
            RotorPosition = 1,
            NotchPosition = 2,
            RingSetting = 20,
            IsRotor = true
        };

        //Reverse
        r2 = new Rotor(new (int From, int To)[]
        {
            (1, 33), (2, 32), (3, 31), (4, 30), (5, 29), (6, 28), (7, 27), (8, 26), (9, 25), (10, 24), (11, 23),
            (12, 22), (13, 21), (14, 20), (15, 19), (16, 18), (17, 17), (18, 16), (19, 15), (20, 14), (21, 13),
            (22, 12), (23, 11), (24, 10), (25, 9), (26, 8), (27, 7), (28, 6), (29, 5), (30, 4), (31, 3), (32, 2),
            (33, 1)
        })
        {
            RotorPosition = 2,
            NotchPosition = 10,
            RingSetting = 8,
            IsRotor = true
        };

        //Half and half
        r3 = new Rotor(new (int From, int To)[]
        {
            (1, 16), (2, 15), (3, 14), (4, 13), (5, 12), (6, 11), (7, 10), (8, 9), (9, 8), (10, 7), (11, 6),
            (12, 5), (13, 4), (14, 3), (15, 2), (16, 1), (33, 17), (32, 18), (31, 19), (30, 20), (29, 21), (28, 22),
            (27, 23), (26, 24), (25, 25), (24, 26), (23, 27), (22, 28), (21, 29), (20, 30), (19, 31), (18, 32), (17, 33)
        })
        {
            RotorPosition = 3,
            NotchPosition = 7,
            RingSetting = 11,
            IsRotor = true
        };

        //Random
        reflector = new Rotor(new (int From, int To)[]
        {
            (1, 7), (2, 24), (3, 12), (4, 23), (5, 18), (6, 17), (7, 1), (8, 19), (9, 22), (10, 21), (11, 31),
            (12, 3), (13, 27), (14, 20), (15, 25), (16, 26), (17, 6), (18, 5), (19, 8), (20, 14), (21, 10), (22, 9),
            (23, 4), (24, 2), (25, 15), (26, 16), (27, 13), (28, 33), (29, 32), (30, 30), (31, 11), (32, 29), (33, 28)
        })
        {
            RotorPosition = 0,
            NotchPosition = 0,
            RingSetting = 0,
            IsRotor = false
        };

        //тыамогус
        plugBoard = new PlugBoard(new (int From, int To)[] { (20, 29), (1, 14), (16, 4), (21, 19) })
        {
            HowManyPlugsUsed = 4,
        };
    }
}

internal class Rotor
{
    public int RotorPosition { get; init; }
    private readonly Dictionary<int, int> _forward;
    private readonly Dictionary<int, int> _reverse;
    public int NotchPosition { get; init; }
    public int RingSetting { get; set; }
    public bool IsRotor { get; init; }

    public Rotor((int From, int To)[] correspondingValues)
    {
        _forward = correspondingValues.ToDictionary(x => x.From, x => x.To);
        _reverse = correspondingValues.ToDictionary(x => x.To, x=> x.From);
    }

    public int EncodeForward(int letterToEncode) => _forward[letterToEncode];

    public int EncodeReverse(int letterToEncode) => _reverse[letterToEncode];

    public bool MoveRotor()
    {
        RingSetting += 1;
        if (RingSetting >= 34)
        {
            RingSetting = 1;
        }

        return RingSetting==NotchPosition;
    }


}

internal class PlugBoard
{
    public int HowManyPlugsUsed { get; init; }

    private readonly Dictionary<int, int> _forward;
    private readonly Dictionary<int, int> _reverse;

    public PlugBoard((int From, int To)[] correspondingValues)
    {
        _forward = correspondingValues.ToDictionary(x => x.From, x => x.To);
        _reverse = correspondingValues.ToDictionary(x => x.To, x => x.From);
    }

    public int EncodeForward(int letterToEncode) =>
        _forward.TryGetValue(letterToEncode, out var val) 
            ? val
            : letterToEncode;

    public int EncodeReverse(int letterToEncode)=>
        _reverse.TryGetValue(letterToEncode, out var val)
            ? val
            : letterToEncode;
}



