namespace MauiAppBazaSportiva;
using MauiAppBazaSportiva.Models;
public partial class ListPage : ContentPage
{
	public ListPage(Member member=null)
	{
		InitializeComponent();
        BindingContext = member ?? new Member();
	}
    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var member = (Member)BindingContext;
        if(string.IsNullOrWhiteSpace(member.FirstName)||string.IsNullOrWhiteSpace(member.LastName))
        {
            await DisplayAlert("Eroare", "Numele si prenumele sunt obligatorii", "OK");
            return;
        }
        await App.Database.SaveMemberAsync(member);

        await Navigation.PopAsync();
    }
    async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var member = (Member)BindingContext;
        await App.Database.DeleteMemberAsync(member);
        await Navigation.PopAsync();
    }
    async void OnChooseButtonClicked(object sender, EventArgs e)
    {
        var member = BindingContext as Member;
        if (member != null)
        {
            var membershipPage = new MembershipPage(new Membership());
            await Navigation.PushAsync(membershipPage);
            membershipPage.Disappearing += async (s, args) =>
            {
                if (membershipPage.SelectedMembership != null)
                {
                    member.MembershipID = membershipPage.SelectedMembership.ID;
                    await App.Database.SaveMemberAsync(member);
                    await DisplayAlert("Succes", "Abonamentul a fost asociat membrului.", "OK");
                    listView.ItemsSource = await App.Database.GetMembersAsync(member.MembershipID);
                }
            };
        }
        else
        {
            await DisplayAlert("Error", "BindingContext nu este setat ca Membru.", "OK");
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if(BindingContext is Member member)
        {
            listView.ItemsSource = await App.Database.GetMembersAsync(member.ID);

        }
        else
        {
            await DisplayAlert("Error", "BindingContext nu e setat catre un membru", "OK");
        }
    }


}