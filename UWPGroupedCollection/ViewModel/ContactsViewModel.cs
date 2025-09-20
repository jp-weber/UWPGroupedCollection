using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPGroupedCollection.Models;
using UWPGroupedCollection.Services;
using Windows.Storage;
using Windows.Storage.Streams;

namespace UWPGroupedCollection.ViewModel;


/// <summary>
/// A viewmodel for a contacts list widget.
/// </summary>
public partial class ContactsViewModel : ObservableObject
{
    public ContactsViewModel()
    {

    }

    /// <summary>
    /// Gets the current collection of contacts
    /// </summary>
    public ObservableGroupedCollection<string, Contact> Contacts { get; private set; } = new();

    /// <summary>
    /// Loads the contacts to display.
    /// </summary>
    [RelayCommand(FlowExceptionsToTaskScheduler = true)]
    private async Task LoadContactsAsync()
    {
        try
        {
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri($"ms-appx:///Assets/randomuser.json"));
            IRandomAccessStreamWithContentType randomStream = await file.OpenReadAsync();
            using StreamReader r = new StreamReader(randomStream.AsStreamForRead());
            var contacts = new SerializationService().Deserialize<ContactsQueryResponse>(await r.ReadToEndAsync());

            Contacts = new ObservableGroupedCollection<string, Contact>(
                contacts.Contacts
                .GroupBy(static c => char.ToUpperInvariant(c.Name.First[0]).ToString())
                .OrderBy(static g => g.Key));

            OnPropertyChanged(nameof(Contacts));
        }
        catch (Exception exc)
        {

            throw;
        }
    }

    /// <summary>
    /// Loads more contacts.
    /// </summary>
    [RelayCommand(FlowExceptionsToTaskScheduler = true)]
    private async Task LoadMoreContactsAsync()
    {

        StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri($"ms-appx:///Assets/randomuser.json"));
        IRandomAccessStreamWithContentType randomStream = await file.OpenReadAsync();
        using StreamReader r = new StreamReader(randomStream.AsStreamForRead());
        var contacts = new SerializationService().Deserialize<ContactsQueryResponse>(await r.ReadToEndAsync());
        //ContactsQueryResponse contacts = await ContactsService.GetContactsAsync(10);

        foreach (Contact contact in contacts.Contacts)
        {
            string key = char.ToUpperInvariant(contact.Name.First[0]).ToString();

            Contacts.InsertItem(
                key: key,
                keyComparer: Comparer<string>.Default,
                item: contact,
                itemComparer: Comparer<Contact>.Create(static (left, right) => Comparer<string>.Default.Compare(left.ToString(), right.ToString())));
        }
    }

    /// <summary>
    /// Removes a given contact from the list.
    /// </summary>
    /// <param name="contact">The contact to remove.</param>
    [RelayCommand]
    private void DeleteContact(Contact contact)
    {
        Contacts.FirstGroupByKey(char.ToUpperInvariant(contact.Name.First[0]).ToString()).Remove(contact);
    }
}
