using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperSolver.NeuralSolver
{
    /// <summary>
    /// http://alishertortay.me/2016/12/22/minesweeper-solver-using-nn/
    /// </summary>
    public class Class1
    {
        /// 9: 0-8
        /// 1: Mine
        /// 1: Flag
        /// 1: Unknown
        /// Total: 12
        /// 11x11x12=1452, 256, 256, and 128
    }


    ////private void Train(Network netModel, Dispatcher dispatcher)
    ////    {
    ////            // q    w    e    r    t    y    u    i    o    p    a    s    d    f    g    h    j    k    l    z    x    c    v    b    n    m  - 26
    ////            // Q    W    E    R    T    Y    U    I    O    P    A    S    D    F    G    H    J    K    L    Z    X    C    V    B    N    M  - 26
    ////            // 0    1    2    3    4    5    6    7    8    9                                                                                  - 10 62

    ////            var localLowestError = double.MaxValue;

    ////    /// Networks
    ////    ActivationNetwork network;

    ////    //network = new DeepBeliefNetwork(new BernoulliFunction(), 1024, 2048, 62);
    ////    //network = new DeepBeliefNetwork(new GaussianFunction(), 1024, 744, 62);
    ////    //network = new DeepBeliefNetwork(new BernoulliFunction(), 1024, 62);
    ////    //network = new ActivationNetwork(new SigmoidFunction(), 1024, 62);

    ////    //var nn = MakeName(network);
    ////    //network.Save(string.Format("{0}\\{1}_{2:yyyyMMdd_HHmmssfff}.ann", nn, localLowestError, DateTime.UtcNow));

    ////    /// Teachers
    ////    //DeepBeliefNetworkLearning teacher = new DeepBeliefNetworkLearning(network)
    ////    //{
    ////    //    Algorithm = (h, v, i) => new ContrastiveDivergenceLearning(h, v)
    ////    //    {
    ////    //        LearningRate = 0.1,
    ////    //        Momentum = 0.5,
    ////    //        Decay = 0.001,
    ////    //    },
    ////    //    LayerIndex = SelectedLayerIndex - 1,
    ////    //};

    ////    //network = DeepBeliefNetwork.Load("C:\\work\\1024_62_20170307_1543.ann");
    ////    var filePath = GetBestSave(netModel.FullPath, out localLowestError);
    ////    network = DeepBeliefNetwork.Load(filePath);

    ////            dispatcher.BeginInvoke((Action<Network, int, double>) UpdateError, DispatcherPriority.ContextIdle, netModel, 0, localLowestError);

    ////            var teacher = new BackPropagationLearning(network)
    ////            {
    ////                LearningRate = 0.1,
    ////                Momentum = 0.9
    ////            };

    ////    //DeepNeuralNetworkLearning teacher = new DeepNeuralNetworkLearning(Main.Network)
    ////    //{
    ////    //    Algorithm = (ann, i) => new ParallelResilientBackpropagationLearning(ann),
    ////    //    LayerIndex = Main.Network.Layers.Length - 1,
    ////    //};

    ////    double[][] inputs, outputs;

    ////    inputs = new double[this.OriginalLetters.Count][];
    ////            outputs = new double[this.OriginalLetters.Count][];

    ////            for (int i = 0; i<inputs.Length; i++)
    ////            {
    ////                //int total = set[i].Database.Classes;

    ////                inputs[i] = this.OriginalLetters[i].Features;
    ////                outputs[i] = new double[62];

    ////                var charCode = (byte)this.OriginalLetters[i].Character;

    ////                if (charCode >= 48 && charCode <= 57)
    ////                {
    ////        // 0..9: 48 -  57 (52 - 61)
    ////        outputs[i][charCode - 48 + 52] = 1;
    ////    }
    ////                else if (charCode >= 65 && charCode <= 90)
    ////                {
    ////        // A..Z: 65 -  90 (26 - 51)
    ////        outputs[i][charCode - 65 + 26] = 1;
    ////    }
    ////                else if (charCode >= 97 && charCode <= 122)
    ////                {
    ////        // a..z: 97 - 122 ( 0 - 25)
    ////        outputs[i][charCode - 97] = 1;
    ////    }
    ////    }

    ////    Utils.Normalize(inputs);

    ////            var cnt = 0;
    ////    var lastOutput = DateTime.UtcNow;
    ////    var networkName = MakeName(network);

    ////            while (true)
    ////            {
    ////                Utils.Randomize(ref inputs, ref outputs);

    ////                // run epoch of learning procedure
    ////                double error = teacher.RunEpoch(inputs, outputs);

    ////                // check error value to see
    ////                if (error< 0.45)
    ////                {
    ////                    System.Diagnostics.Debug.WriteLine("learned at {0}", cnt);

    ////                    break;
    ////                }

    ////                if (network is DeepBeliefNetwork)
    ////                {
    ////                    ((DeepBeliefNetwork) network).UpdateVisibleWeights();
    ////                }

    ////                if ((DateTime.UtcNow - lastOutput).TotalSeconds > 5)
    ////                {
    ////                    lastOutput = DateTime.UtcNow;

    ////                    System.Diagnostics.Debug.WriteLine("{2:yyyy-MM-dd HH:mm:ss.fff} {0}. error: {1}", cnt, error, lastOutput);

    ////                    dispatcher.BeginInvoke((Action<Network, int, double>) UpdateError, DispatcherPriority.ContextIdle, netModel, cnt + 1, error);
    ////                }

    ////                if (localLowestError > error)
    ////                {
    ////                    if (!Directory.Exists(networkName))
    ////                    {
    ////                        Directory.CreateDirectory(networkName);
    ////                    }

    ////                    network.Save(string.Format("{0}\\{1}_{2:yyyyMMdd_HHmmssfff}.ann", networkName, error, DateTime.UtcNow));

    ////                    localLowestError = error;

    ////                    dispatcher.BeginInvoke((Action<Network, int, double>) UpdateError, DispatcherPriority.ContextIdle, netModel, cnt + 1, error);
    ////                }

    ////                cnt++;
    ////            }
    ////        }

    ////        public void StopTraining(Network network)
    ////{
    ////    var source = network.TokenSource;

    ////    network.TrainingTask = null;
    ////    network.TokenSource = null;

    ////    source.Cancel();
    ////}

    ////private void UpdateError(Network network, int epoch, double error)
    ////{
    ////    network.CurrentEpoch = epoch;
    ////    network.CurrentError = error;
    ////}

    ////public static string GetBestSave(string networkSavePath, out double minError)
    ////{
    ////    var fileNames = Directory.EnumerateFiles(networkSavePath, "*.ann");

    ////    minError = double.MaxValue;
    ////    var minFileName = string.Empty;

    ////    foreach (var fileName in fileNames)
    ////    {
    ////        var fileNameParts = Path.GetFileName(fileName).Split('_');

    ////        if (double.TryParse(fileNameParts[0].Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double errorValue))
    ////        {
    ////            if (errorValue < minError)
    ////            {
    ////                minError = errorValue;
    ////                minFileName = fileName;
    ////            }
    ////        }
    ////    }

    ////    return minFileName;
    ////}

    ////private static string MakeName(ActivationNetwork network)
    ////{
    ////    var layers = network.Layers;
    ////    var name = string.Empty;
    ////    for (int i = 0; i < layers.Length; i++)
    ////    {
    ////        name += string.Format("{0}_", layers[i].InputsCount);

    ////        if (i == layers.Length - 1)
    ////        {
    ////            name += string.Format("{0}", layers[i].Neurons.Length);
    ////        }
    ////    }

    ////    return name;
    ////}

    ////private static double[] ToFeatures(Bitmap bmp)
    ////{
    ////    var features = new double[32 * 32];

    ////    for (int i = 32 * 32 - 1; i >= 0; i--)
    ////    {
    ////        features[i] = 255;
    ////    }

    ////    for (int i = 0; i < bmp.Height; i++)
    ////        for (int j = 0; j < bmp.Width; j++)
    ////            features[i * 32 + j] = (bmp.GetPixel(j, i).R > 0) ? 0 : 1;

    ////    return features;
    ////}
}
