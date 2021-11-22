﻿using Chat_App_Database.Database;
using Chat_App_Library.Interfaces;
using Chat_App_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Chat_App_Logic.Repositories
{
    public class ChatDbContextRepository : IRepository
    {
        private ChatDbContext _dbContext;

        private static ChatDbContextRepository _instance;

        public ChatDbContextRepository()
        {
            _dbContext = new ChatDbContext();
        }

        public static ChatDbContextRepository GetSingleton()
        {
            if (_instance == null)
            {
                _instance = new ChatDbContextRepository();
            }
            return _instance;
        }

        public void ClearAllDataSets()
        {
            foreach (var item in _dbContext.GeneralChatDatabase)
            {
                _dbContext.GeneralChatDatabase.Remove(item);
            }
            foreach (var item in _dbContext.GroupChatDatabase)
            {
                _dbContext.GroupChatDatabase.Remove(item);
            }
            foreach (var item in _dbContext.MessageDatabase)
            {
                _dbContext.MessageDatabase.Remove(item);
            }
            foreach (var item in _dbContext.SingleUserChatDatabase)
            {
                _dbContext.SingleUserChatDatabase.Remove(item);
            }
            foreach (var item in _dbContext.UserDatabase)
            {
                _dbContext.UserDatabase.Remove(item);
            }
        }
        public List<RefreshToken> GetAllRefreshTokens()
        {
            return _dbContext.RefreshTokens.ToList();
        }
        public void UpdateRefreshToken(RefreshToken token)
        {
            var foundtoken = _dbContext.RefreshTokens.Where(a => a.Id == token.Id).FirstOrDefault();
            foundtoken.AddedDate = token.AddedDate;
            foundtoken.ExpiryDate = token.ExpiryDate;
            foundtoken.Id = token.Id;
            foundtoken.IsRevoked = token.IsRevoked;
            foundtoken.IsUsed = token.IsUsed;
            foundtoken.jwtId = token.jwtId;
            foundtoken.Token = token.Token;
            foundtoken.User = token.User;
            foundtoken.UserId = token.UserId;
            _dbContext.SaveChanges();
        }
        public async Task AddRefreshToken(RefreshToken token)
        {
            await _dbContext.RefreshTokens.AddAsync(token);
            _dbContext.SaveChanges();
        }

        public void AddUser(User user)
        {
            _dbContext.UserDatabase.Add(user);
            _dbContext.SaveChanges();
        }

        public void AddGroupChat(GroupChat groupChat)
        {
            _dbContext.GroupChatDatabase.Add(groupChat);
            _dbContext.SaveChanges();
        }

        public void AddSingleUserChat(SingleUserChat singleUserChat)
        {
            _dbContext.SingleUserChatDatabase.Add(singleUserChat);
            _dbContext.SaveChanges();
        }

        public void AddGeneralChat(GeneralChat generalChat)
        {
            
            _dbContext.GeneralChatDatabase.Add(generalChat);
            _dbContext.SaveChanges();
        }


        public List<User> GetUsers()
        {
            return _dbContext.UserDatabase.ToList();
        }

        public List<GroupChat> GetGroupChats()
        {
            return _dbContext.GroupChatDatabase.ToList();
        }

        public List<GeneralChat> GetGeneralChat()
        {
            return _dbContext.GeneralChatDatabase.ToList();
        }

        public List<SingleUserChat> GetSingleUserChat()
        {
            return _dbContext.SingleUserChatDatabase.ToList();
        }

        public List<Message> GetMessages()
        {
            return _dbContext.MessageDatabase.ToList();
        }

        public List<Message> GetMessagesByUserId(Expression<Func<Message, bool>> id)
        {
            IQueryable<Message> query = _dbContext.MessageDatabase as IQueryable<Message>;
            return query.Where(id).ToList();
        }
#nullable enable
        public User? GetUserById(Expression<Func<User, bool>> id)
        {
            IQueryable<User> query = _dbContext.UserDatabase.AsQueryable();
            return query.Where(id).FirstOrDefault();
        }

        public List<User> GetUserByName(Expression<Func<User, bool>> name)
        {
            IQueryable<User> query = _dbContext.UserDatabase.AsQueryable();
            return query.Where(name).ToList();
        }

        public List<GroupChat> GetGroupChatsByUserId(Expression<Func<GroupChat, bool>> id)
        {
            IQueryable<GroupChat> query = _dbContext.GroupChatDatabase.AsQueryable();
            return query.Where(id).ToList();
        }

        public List<SingleUserChat> GetSingleUserChatByUserId(Expression<Func<SingleUserChat, bool>> id)
        {
            IQueryable<SingleUserChat> query = _dbContext.SingleUserChatDatabase.AsQueryable();
            return query.Where(id).ToList();
        }

        public void UpdateUserData(User userupdate)
        {
            var user = _dbContext.UserDatabase.Where(a => a.Id == userupdate.Id).FirstOrDefault();
            if (!(user == null))
            {
                user.Email = userupdate.Email;
                user.Name = userupdate.Name;
                user.HashBase64 = userupdate.HashBase64;
                user.Salt = userupdate.Salt;
                user.Invitations = userupdate.Invitations;
                user.Role = userupdate.Role;
                user.Username = userupdate.Username;
            }
            _dbContext.SaveChanges();
        }
#nullable disable

        public void AddMessageToGroupChat(Message message, Expression<Func<GroupChat, bool>> id)
        {
            IQueryable<GroupChat> query = _dbContext.GroupChatDatabase.AsQueryable();
            query.FirstOrDefault(id).Messages.Add(message);
            _dbContext.MessageDatabase.Add(message);
            _dbContext.SaveChanges();
        }


        public void AddMessageToSingleUserChat(Message message, Expression<Func<SingleUserChat, bool>> id)
        {
            IQueryable<SingleUserChat> query = _dbContext.SingleUserChatDatabase.AsQueryable();
            query.FirstOrDefault(id)
            .Messages.Add(message);
            _dbContext.MessageDatabase.Add(message);
            _dbContext.SaveChanges();
        }

        public void AddMessageToGeneralChat(Message message, Expression<Func<GeneralChat, bool>> id)
        {
            IQueryable<GeneralChat> query = _dbContext.GeneralChatDatabase.AsQueryable();
            query.FirstOrDefault(id).Messages.Add(message);
            _dbContext.MessageDatabase.Add(message);
            _dbContext.SaveChanges();
        }

        public void DeleteUser(int id)
        {
            _dbContext.UserDatabase.Remove(_dbContext.UserDatabase.Where(a => a.Id == id).FirstOrDefault());
            _dbContext.SaveChanges();
        }

        public void DeleteGroup(GroupChat chat)
        {
            _dbContext.GroupChatDatabase.Remove(chat);
            _dbContext.SaveChanges();
        }

        public void DeleteGeneralChat(GeneralChat chat)
        {
            _dbContext.GeneralChatDatabase.Remove(chat);
            _dbContext.SaveChanges();
        }

        public void DeleteSiglePersonChat(SingleUserChat chat)
        {
            _dbContext.SingleUserChatDatabase.Remove(chat);
            _dbContext.SaveChanges();
        }

        public void DeleteMessageGroup(GroupChat chat, int messageid)
        {
            _dbContext.GroupChatDatabase.Where(a => a.Id == chat.Id).FirstOrDefault().
            Messages.Remove(_dbContext.GroupChatDatabase.Where(a => a.Id == chat.Id).FirstOrDefault().
            Messages.Where(a => a.Id == messageid).FirstOrDefault());
            _dbContext.SaveChanges();
        }

        public void DeleteMessageGeneral(GeneralChat chat, int messageid)
        {
            _dbContext.GeneralChatDatabase.Where(a => a.Id == chat.Id).FirstOrDefault().
            Messages.Remove(_dbContext.GeneralChatDatabase.Where(a => a.Id == chat.Id).FirstOrDefault().
            Messages.Where(a => a.Id == messageid).FirstOrDefault());
            _dbContext.SaveChanges();
        }

        public void DeleteMessageSingleUser(SingleUserChat chat, int messageid)
        {
            _dbContext.SingleUserChatDatabase.Where(a => a.Id == chat.Id).FirstOrDefault().
            Messages.Remove(_dbContext.SingleUserChatDatabase.Where(a => a.Id == chat.Id).FirstOrDefault().
            Messages.Where(a => a.Id == messageid).FirstOrDefault());
            _dbContext.SaveChanges();
        }

        public void UpdateMessageToGroupChat(Message message, int groupid)
        {
            var item = _dbContext.GroupChatDatabase.FirstOrDefault(a => a.Id == groupid)
            .Messages.Where(a => a.Id == message.Id).FirstOrDefault();
            item.EndDate = DateTime.Now;
            item.Text = message.Text;
            var item2 = _dbContext.MessageDatabase.FirstOrDefault(a => a.Id == message.Id);
            item2.EndDate = DateTime.Now;
            item2.Text = message.Text;
            _dbContext.SaveChanges();
        }


        public void UpdateMessageToSingleUserChat(Message message, int singleuserchatid)
        {
            var item = _dbContext.SingleUserChatDatabase.FirstOrDefault(a => a.Id == singleuserchatid)
            .Messages.Where(a => a.Id == message.Id).FirstOrDefault();
            item.EndDate = DateTime.Now;
            item.Text = message.Text;
            var item2 = _dbContext.MessageDatabase.FirstOrDefault(a => a.Id == message.Id);
            item2.EndDate = DateTime.Now;
            item2.Text = message.Text;
            _dbContext.SaveChanges();
        }

        public void UpdateMessageToGeneralChat(Message message, int groupchatid)
        {
            var item = _dbContext.GeneralChatDatabase.FirstOrDefault(a => a.Id == groupchatid)
            .Messages.Where(a => a.Id == message.Id).FirstOrDefault();
            item.EndDate = DateTime.Now;
            item.Text = message.Text;
            var item2 = _dbContext.MessageDatabase.FirstOrDefault(a => a.Id == message.Id);
            item2.EndDate = DateTime.Now;
            item2.Text = message.Text;
            _dbContext.SaveChanges();
        }

        public void BlockUserFromGeneralChat(int UserId, GeneralChat Chat)
        {
            var User = _dbContext.UserDatabase.Where(a => a.Id == UserId).FirstOrDefault();
            var ChatToUpdate = _dbContext.GeneralChatDatabase.Where(a => a.Id == Chat.Id).FirstOrDefault();
            ChatToUpdate.BannedUsers = Chat.BannedUsers;
            ChatToUpdate.BannedUsers.Add(User);
            ChatToUpdate.ChatBanned = Chat.ChatBanned;
            ChatToUpdate.CreationDate = Chat.CreationDate;
            ChatToUpdate.Id = Chat.Id;
            ChatToUpdate.MaxAmountPersons = Chat.MaxAmountPersons;
            ChatToUpdate.Messages = Chat.Messages;
            ChatToUpdate.Owner = Chat.Owner;
            ChatToUpdate.Password = Chat.Password;
            ChatToUpdate.Private = Chat.Private;
            ChatToUpdate.Title = Chat.Title;
            _dbContext.SaveChanges();
        }

        public void BlockUserFromGroupChat(int UserId, GroupChat Chat)
        {
            var User = _dbContext.UserDatabase.Where(a => a.Id == UserId).FirstOrDefault();
            var ChatToUpdate = _dbContext.GroupChatDatabase.Where(a => a.Id == Chat.Id).FirstOrDefault();
            ChatToUpdate.BannedUsers = Chat.BannedUsers;
            ChatToUpdate.BannedUsers.Add(User);
            ChatToUpdate.ChatBanned = Chat.ChatBanned;
            ChatToUpdate.CreationDate = Chat.CreationDate;
            ChatToUpdate.Id = Chat.Id;
            ChatToUpdate.MaxAmountPersons = Chat.MaxAmountPersons;
            ChatToUpdate.Messages = Chat.Messages;
            ChatToUpdate.GroupOwner = Chat.GroupOwner;
            ChatToUpdate.HashBase64 = Chat.HashBase64;
            ChatToUpdate.Private = Chat.Private;
            ChatToUpdate.Title = Chat.Title;
            _dbContext.SaveChanges();
        }

        public void AddUserToGroupChat(int UserId, GroupChat Chat)
        {
            var User = _dbContext.UserDatabase.Where(a => a.Id == UserId).FirstOrDefault();
            var ChatToUpdate = _dbContext.GroupChatDatabase.Where(a => a.Id == Chat.Id).FirstOrDefault();
            ChatToUpdate.Users.Add(User);
            _dbContext.SaveChanges();
        }
    }
}
