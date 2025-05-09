﻿using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfEscapeGame;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    // enumerations
    public enum MessageTypes
    {
        Info,
        Warning,
        Error
    }
    Room currentRoom; // will become useful in later versions
    public MainWindow()
    {
        InitializeComponent();

        // define room
        Room bedroom = new Room()
        {
            Name = "bedroom",
            Description = "I seem to be in a medium sized bedroom.There is a locker to the left,a nice rugon the floor,and a bed to the right. ",
            Image = new ()
            {
                Source = new BitmapImage(new Uri("media/ss-bedroom.png", UriKind.Relative))
            }
        };

        Room livingroom = new Room()
        {
            Name = "living room",
            Description = "The living room is a place in the house where we can chill.",
            Image = new ()
            {
                Source = new BitmapImage(new Uri("media/ss-living.png", UriKind.Relative))
            }
        };

        Room computerroom = new Room()
        {
            Name = "computer room",
            Description = "The computer room is a place in the house where we can work for school.",
            Image = new ()
            {
                Source = new BitmapImage(new Uri("media/ss-computer.png", UriKind.Relative))
            }
        };

        // define items
        Item key1 = new ()
        {
            Name = "small silver key",
            Description = " A small silver key, makes me think of one I had at the highschool",
            IsPortable = true
        };

        Item key2 = new ()
        {
            Name = "large key",
            Description = "A large key. Could this be my way out? ",
            IsPortable = true
        };

        Item locker = new ()
        {
            Name = "locker",
            Description = "A locker with a keyhole. ",
            IsLocked = true,
        };

        Item chair = new ()
        {
            Name = "chair",
            Description = "A chair to sit on",
        };

        Item poster = new ()
        {
            Name = "poster",
            Description = "A poster of a band I don't know. ",
            IsPortable = true
        };

        Item floormat = new ()
        {
            Name = "poster",
            Description = "A poster of a band I don't know. ",
            IsPortable = true
        };

        Item plant = new ()
        {
            Name = "plant",
            Description = "A plant that needs his daily water",
            IsPortable = true
        };

        Item sofa = new ()
        {
            Name = "sofa",
            Description = "A seat where we all can sit on.zd",
            IsPortable = false
        };

        Item computer = new ()
        {
            Name = "computer",
            Description = "Technologie where we can game or study on",
            IsPortable = true
        };

        Item heater = new ()
        {
            Name = "heater",
            Description = "A source of heat to warm",
            IsPortable = true
        };

        locker.HiddenItem = key2;
        locker.IsLocked = true;
        locker.Key = key1;
        Item bed = new ()
        {
            Name = "bed",
            Description = "Just a bed. I am not tired now.",
        };
        bed.HiddenItem = key1;

        // setup bedroom
        bedroom.Items.Add(new Item()
        {
            Name = "floor mat",
            Description = "A bit ragged floor mat, but still one of the most popular designs. "
        });
        bedroom.Items.Add(bed);
        bedroom.Items.Add(locker);

        // setup living room
        livingroom.Items.Add(sofa);
        livingroom.Items.Add(plant);
        livingroom.Items.Add(floormat);

        // setup computer room
        computerroom.Items.Add(computer);
        computerroom.Items.Add(heater);
        computerroom.Items.Add(chair);
        computerroom.Items.Add(poster);

        // setup doors
        // door from bedroom to living room
        Door bedroomToLiving = new ()
        {
            Name = "living room door",
            Description = "A door to the living room",
            ToRoom = livingroom
        };

        // door from living to computer room
        Door livingToComputer = new ()
        {
            Name = "computer room door",
            Description = "A door to the computer room",
            ToRoom = computerroom
        };

        // door from computer to living room
        Door computerToLiving = new ()
        {
            Name = "living room door",
            Description = "A door to the living room",
            ToRoom = livingroom
        };

        // door from living to bedroom
        Door livingToBedroom = new ()
        {
            Name = "bedroom door",
            Description = "A door to the bedroom",
            ToRoom = bedroom
        };

        // add doors to rooms
        bedroom.Doors.Add(bedroomToLiving);
        livingroom.Doors.Add(livingToComputer);
        livingroom.Doors.Add(livingToBedroom);
        computerroom.Doors.Add(computerToLiving);

        // start game
        currentRoom = bedroom;
        txtMessage.Text = "I am awake, but remember who I am!? Must have been a hell of party last night... ";
        txtRoomDesc.Text = currentRoom.Description;
        UpdateUI();
    }

    /// <summary>
    /// Update de items in de ListBoxes
    /// </summary>
    private void UpdateUI()
    {
        lstRoomItems.Items.Clear();
        foreach (Item itm in currentRoom.Items)
        {
            lstRoomItems.Items.Add(itm);
        }

        lstRoomDoor.Items.Clear();
        foreach (Door door in currentRoom.Doors)
        {
            lstRoomDoor.Items.Add(door);
        }

        // update img
        imgRoom.Source = currentRoom.Image.Source;
    }

    private void LstItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        btnCheck.IsEnabled = lstRoomItems.SelectedValue != null; // room item selected
        btnPickUp.IsEnabled = lstRoomItems.SelectedValue != null; // room item selected
        btnUseOn.IsEnabled = lstRoomItems.SelectedValue != null && lstMyItems.SelectedValue != null; // room item and picked up item selected
    }

    private void BtnCheck_Click(object sender, RoutedEventArgs e)
    {
        // 1. find item to check
        Item roomItem = (Item)lstRoomItems.SelectedItem;

        // 2. is it locked?
        if (roomItem.IsLocked)
        {
            txtMessage.Text = roomItem.Description + RandomMessageGenerator.GetRandomMessage(MessageType.Locked);
            return;
        }

        // 3. does it contain a hidden item?
        Item foundItem = roomItem.HiddenItem;
        if (foundItem != null)
        {
            txtMessage.Text = $"Oh, look, I found a {foundItem.Name}. ";
            lstMyItems.Items.Add(foundItem);
            roomItem.HiddenItem = null;
            return;
        }

        // 4. just another item; show description
        txtMessage.Text = roomItem.Description;
    }

    private void BtnUseOn_Click(object sender, RoutedEventArgs e)
    {
        // 1. find both items
        Item myItem = (Item)lstMyItems.SelectedItem;
        Item roomItem = (Item)lstRoomItems.SelectedItem;

        // 2. item doesn't fit
        if (roomItem.Key != myItem)
        {
            txtMessage.Text = RandomMessageGenerator.GetRandomMessage(MessageType.CantUse);
            return;
        }

        // 3. item fits; other item unlocked
        roomItem.IsLocked = false;
        roomItem.Key = null;
        lstMyItems.Items.Remove(myItem);
        txtMessage.Text = $"I just unlocked the {roomItem.Name}!";
    }

    private void BtnPickUp_Click(object sender, RoutedEventArgs e)
    {
        // 1. find selected item
        Item selItem = (Item)lstRoomItems.SelectedItem;

        // is it portbable?
        if (!selItem.IsPortable)
        {
            txtMessage.Text = RandomMessageGenerator.GetRandomMessage(MessageType.CantPickUp) + selItem.Name;
        }
        else
        {
            // 2. add item to your items list
            txtMessage.Text = $"I just picked up the {selItem.Name}. ";
            lstMyItems.Items.Add(selItem);
            lstRoomItems.Items.Remove(selItem);
            currentRoom.Items.Remove(selItem);
        }
    }

    private void BtnOpenWith_Click(object sender, RoutedEventArgs e)
    {
        // 1. find selected item
        Item selItem = (Item)lstRoomItems.SelectedItem;

        // is it portbable?
        if (!selItem.IsPortable)
        {
            txtMessage.Text = RandomMessageGenerator.GetRandomMessage(MessageType.CantPickUp) + selItem.Name;
        }
        else
        {
            // 2. add item to your items list
            txtMessage.Text = $"I just picked up the {selItem.Name}. ";
            lstMyItems.Items.Add(selItem);
            lstRoomItems.Items.Remove(selItem);
            currentRoom.Items.Remove(selItem);
        }
    }

    private void BtnDrop_Click(object sender, RoutedEventArgs e)
    {
        // 1. find selected item
        Item selItem = (Item)lstMyItems.SelectedItem;

        // 2. add item to your items list
        txtMessage.Text = $"I just dropped the {selItem.Name}. ";
        lstRoomItems.Items.Add(selItem);
        lstMyItems.Items.Remove(selItem);
        currentRoom.Items.Add(selItem);
    }

    private void LstRoomDoor_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        btnEnter.IsEnabled = lstRoomDoor.SelectedValue != null; // room item selected
    }

    private void BtnEnter_Click(object sender, RoutedEventArgs e)
    {
        // 1. find selected door
        Door selDoor = (Door)lstRoomDoor.SelectedItem;

        // 2. is it locked?
        if (selDoor.IsLocked)
        {
            txtMessage.Text = RandomMessageGenerator.GetRandomMessage(MessageType.Locked);
            return;
        }

        // 3. go to next room
        currentRoom = selDoor.ToRoom;
        txtRoomDesc.Text = currentRoom.Description;
        UpdateUI();
    }
}