<%@ Page Language="C#" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <% var highScore = new PTCHighScoreService.HighScoreService(); %>
            <% int i = 0; %>
            <% foreach(var score in highScore.GetCurrentHighScores()) { %>
            <tr>
                <% i++; %>
                <%= i.ToString() + ". " + score.Name + ", " + score.Country + "   " + score.Score.ToString() + " Points" %>
            </tr>            
            <% } %>
            
        </table>    
    </div>
    </form>
</body>
</html>
