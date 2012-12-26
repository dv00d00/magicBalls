namespace MagicBall
{
    using System;
    using System.Windows;
    using System.Windows.Media.Animation;

    using Microsoft.Phone.Controls;

    public partial class MainPage : PhoneApplicationPage
    {
        private readonly Random random = new Random();

        private readonly Duration duration = new Duration(TimeSpan.FromSeconds(0.5));

        private string[] answers;

        // Constructor
        public MainPage()
        {
            this.InitializeComponent();

            var shakeDetector = new AccelerometerSensorWithShakeDetection();

            this.answers = new[]
                {
                    "Бесспорно",
                    "Предрешено",
                    "Никаких сомнений",
                    "Определённо да",
                    "Можешь быть уверен",                    
                    " Мне кажется да",
                    "Вероятнее всего",
                    "Хорошие перспективы",
                    "Знаки говорят да",
                    "Пока не ясно",
                    "Спроси позже",
                    "Лучше не говорить",
                    "Сейчас нельзя предсказать",
                    "Сконцен-трируйся",
                    " Мой ответ нет",
                    "Думаю что нет",
                    "Весьма сомнительно",
                    "Врядли"
                };

            this.Loaded += (sender, args) =>
            {
                shakeDetector.Start();
                shakeDetector.ShakeDetected += this.OnShakeDetected;
            };

            this.Unloaded += (sender, args) =>
            {
                shakeDetector.ShakeDetected -= this.OnShakeDetected;
                shakeDetector.Stop();
            };
        }

        private void OnShakeDetected(object sender, EventArgs args)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                var sb = this.Start();
                sb.Completed += (o, eventArgs) => this.Stop();
            });
        }

        private int RandomSignum()
        {
            return this.random.Next(1, 3) != 1 ? 1 : -1;
        }

        private void ApplicationBarIconButton_OnStartClick(object sender, EventArgs e)
        {
            this.Start();
        }

        private Storyboard Start()
        {
            var doubleAnimationScaleX = new DoubleAnimation { AutoReverse = false, Duration = this.duration, From = 1, To = 0.7 };

            var doubleAnimationScaleY = new DoubleAnimation { AutoReverse = false, Duration = this.duration, From = 1, To = 0.7 };

            var doubleAnimationRotate = new DoubleAnimation
            {
                AutoReverse = false,
                Duration = this.duration,
                From = 0,
                To = this.RandomSignum() * this.random.Next(45, 90)
            };

            var opacityAnimation = new DoubleAnimation { AutoReverse = false, Duration = this.duration, From = 0, To = 0.5, };

            var translateX = new DoubleAnimation
            {
                AutoReverse = false,
                Duration = this.duration,
                From = 0,
                To = this.RandomSignum() * this.random.Next(75, 125),
            };

            var translateY = new DoubleAnimation
            {
                AutoReverse = false,
                Duration = this.duration,
                From = 0,
                To = this.RandomSignum() * this.random.Next(75, 125),
            };

            var ballShakeAnimationX = new DoubleAnimationUsingKeyFrames { Duration = duration};
            ballShakeAnimationX.KeyFrames.Add(new EasingDoubleKeyFrame
                                {
                                    KeyTime = TimeSpan.FromSeconds(0.1),
                                    Value = this.RandomSignum() * random.Next(1, 3)
                                });
            
            ballShakeAnimationX.KeyFrames.Add(new EasingDoubleKeyFrame
                                {
                                    KeyTime = TimeSpan.FromSeconds(0.3),
                                    Value = this.RandomSignum() * random.Next(1, 3)
                                });
            ballShakeAnimationX.KeyFrames.Add(new EasingDoubleKeyFrame
                                {
                                    KeyTime = TimeSpan.FromSeconds(0.5),
                                    Value = 0
                                });
            
            var ballShakeAnimationY = new DoubleAnimationUsingKeyFrames { Duration = duration};
            
            ballShakeAnimationY.KeyFrames.Add(new EasingDoubleKeyFrame
                                {
                                    KeyTime = TimeSpan.FromSeconds(0.1),
                                    Value = this.RandomSignum() * random.Next(10, 20)
                                });

            ballShakeAnimationY.KeyFrames.Add(new EasingDoubleKeyFrame
                                {
                                    KeyTime = TimeSpan.FromSeconds(0.3),
                                    Value = this.RandomSignum() * random.Next(10, 20)
                                });
           
            ballShakeAnimationY.KeyFrames.Add(new EasingDoubleKeyFrame
                                {
                                    KeyTime = TimeSpan.FromSeconds(0.5),
                                    Value = 0
                                });
            
            var maskShakeAnimationX = new DoubleAnimationUsingKeyFrames { Duration = duration};
            maskShakeAnimationX.KeyFrames.Add(new EasingDoubleKeyFrame
                                {
                                    KeyTime = TimeSpan.FromSeconds(0.1),
                                    Value = ballShakeAnimationX.KeyFrames[0].Value
                                });

            maskShakeAnimationX.KeyFrames.Add(new EasingDoubleKeyFrame
                                {
                                    KeyTime = TimeSpan.FromSeconds(0.3),
                                    Value = ballShakeAnimationX.KeyFrames[1].Value
                                });
            maskShakeAnimationX.KeyFrames.Add(new EasingDoubleKeyFrame
                                {
                                    KeyTime = TimeSpan.FromSeconds(0.5),
                                    Value = 0
                                });

            var maskShakeAnimationY = new DoubleAnimationUsingKeyFrames { Duration = duration };

            maskShakeAnimationY.KeyFrames.Add(new EasingDoubleKeyFrame
                                {
                                    KeyTime = TimeSpan.FromSeconds(0.1),
                                    Value = ballShakeAnimationY.KeyFrames[0].Value
                                });

            maskShakeAnimationY.KeyFrames.Add(new EasingDoubleKeyFrame
                                {
                                    KeyTime = TimeSpan.FromSeconds(0.3),
                                    Value = ballShakeAnimationY.KeyFrames[1].Value
                                });

            maskShakeAnimationY.KeyFrames.Add(new EasingDoubleKeyFrame
                                {
                                    KeyTime = TimeSpan.FromSeconds(0.5),
                                    Value = 0
                                });

            var sb = new Storyboard();
            sb.Children.Add(doubleAnimationScaleX);
            sb.Children.Add(doubleAnimationScaleY);
            sb.Children.Add(doubleAnimationRotate);
            sb.Children.Add(opacityAnimation);
            sb.Children.Add(translateX);
            sb.Children.Add(translateY);
            sb.Children.Add(ballShakeAnimationX);
            sb.Children.Add(ballShakeAnimationY);            
            sb.Children.Add(maskShakeAnimationX);
            sb.Children.Add(maskShakeAnimationY);
            sb.Duration = this.duration;

            Storyboard.SetTarget(doubleAnimationScaleX, this.transform);
            Storyboard.SetTarget(doubleAnimationScaleY, this.transform);
            Storyboard.SetTarget(doubleAnimationRotate, this.transform);
            Storyboard.SetTarget(opacityAnimation, this.opacityMask);
            Storyboard.SetTarget(translateX, this.transform);
            Storyboard.SetTarget(translateY, this.transform);
            Storyboard.SetTarget(ballShakeAnimationX, this.shakeTransform);
            Storyboard.SetTarget(ballShakeAnimationY, this.shakeTransform);            
            Storyboard.SetTarget(maskShakeAnimationX, this.maskTransform);
            Storyboard.SetTarget(maskShakeAnimationY, this.maskTransform);

            Storyboard.SetTargetProperty(doubleAnimationScaleX, new PropertyPath("ScaleX"));
            Storyboard.SetTargetProperty(doubleAnimationScaleY, new PropertyPath("ScaleY"));
            Storyboard.SetTargetProperty(doubleAnimationRotate, new PropertyPath("Rotation"));
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath("Opacity"));

            Storyboard.SetTargetProperty(translateX, new PropertyPath("TranslateX"));
            Storyboard.SetTargetProperty(translateY, new PropertyPath("TranslateY"));

            Storyboard.SetTargetProperty(ballShakeAnimationX, new PropertyPath("SkewX"));
            Storyboard.SetTargetProperty(ballShakeAnimationY, new PropertyPath("TranslateY"));            
            Storyboard.SetTargetProperty(maskShakeAnimationX, new PropertyPath("SkewX"));
            Storyboard.SetTargetProperty(maskShakeAnimationY, new PropertyPath("TranslateY"));

            sb.Begin();

            return sb;
        }

        private void ApplicationBarIconButton_OnStopClick(object sender, EventArgs e)
        {
            this.Stop();
        }

        private Storyboard Stop()
        {
            var doubleAnimationScaleX = new DoubleAnimation { AutoReverse = false, Duration = this.duration, From = 0.7, To = 1 };

            var doubleAnimationScaleY = new DoubleAnimation { AutoReverse = false, Duration = this.duration, From = 0.7, To = 1 };

            var doubleAnimationRotate = new DoubleAnimation
            {
                AutoReverse = false,
                Duration = this.duration,
                From = this.RandomSignum() * this.random.Next(45, 90),
                To = 0
            };

            var opacityAnimation = new DoubleAnimation { AutoReverse = false, Duration = this.duration, From = 0.5, To = 0, };

            var translateX = new DoubleAnimation
            {
                AutoReverse = false,
                Duration = this.duration,
                From = this.RandomSignum() * this.random.Next(75, 125),
                To = 0
            };

            var translateY = new DoubleAnimation
            {
                AutoReverse = false,
                Duration = this.duration,
                From = this.RandomSignum() * this.random.Next(75, 125),
                To = 0
            };

            var sb = new Storyboard();
            sb.Children.Add(doubleAnimationScaleX);
            sb.Children.Add(doubleAnimationScaleY);
            sb.Children.Add(doubleAnimationRotate);
            sb.Children.Add(opacityAnimation);
            sb.Children.Add(translateX);
            sb.Children.Add(translateY);
            sb.Duration = this.duration;

            Storyboard.SetTarget(doubleAnimationScaleX, this.transform);
            Storyboard.SetTarget(doubleAnimationScaleY, this.transform);
            Storyboard.SetTarget(doubleAnimationRotate, this.transform);
            Storyboard.SetTarget(opacityAnimation, this.opacityMask);
            Storyboard.SetTarget(translateX, this.transform);
            Storyboard.SetTarget(translateY, this.transform);

            Storyboard.SetTargetProperty(doubleAnimationScaleX, new PropertyPath("ScaleX"));
            Storyboard.SetTargetProperty(doubleAnimationScaleY, new PropertyPath("ScaleY"));
            Storyboard.SetTargetProperty(doubleAnimationRotate, new PropertyPath("Rotation"));
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath("Opacity"));

            Storyboard.SetTargetProperty(translateX, new PropertyPath("TranslateX"));
            Storyboard.SetTargetProperty(translateY, new PropertyPath("TranslateY"));

            this.result.Text = this.answers[this.random.Next(0, this.answers.Length)];

            sb.Begin();

            return sb;
        }
    }
}