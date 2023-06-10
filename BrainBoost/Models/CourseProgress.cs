using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainBoost.Models
{
    public interface IObserver
    {
        void Update(CourseProgress courseProgress);
    }

    public interface ISubject
    {
        void RegisterObserver(IObserver observer);
        void RemoveObserver(IObserver observer);
        void NotifyObservers();
    }
    public class CourseProgress: ISubject
    {
        [Key]
        public int CourseProgressId { get; set; }

        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public Student Student { get; set; }

        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public Course Course { get; set; }

        [DisplayName("Last accessed")]
        public DateTime? LastAccess { get; set; }

        [Display(Name = "Progress")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:P0}")]
        public double Progress { get; set; }

        [Display(Name = "Is completed")]
        public bool IsCompleted { get; set; }

        [Display(Name = "Hours")]
        public int Hours { get; set; }

        public CourseProgress() {
            _observers = new List<IObserver>();
        }

        private IList<IObserver> _observers;


        public void RegisterObserver(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void RemoveObserver(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void NotifyObservers()
        {
            foreach (var observer in _observers)
            {
                observer.Update(this);
            }
        }

        public void UpdateProgress(double newProgress)
        {
            this.Progress = newProgress;

            if (this.Progress >= 1.0)
            {
                this.IsCompleted = true;
            }
            NotifyObservers();
        }
    }
}
