<%@ Page Language="C#" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <tr>
                <td>
                    Number
                </td>
                <td>
                    Name
                </td>
                <td>
                    Country
                </td>
                <td>
                    Score
                </td>
            </tr>
            <% var highScore = new PTCHighScoreService.HighScoreService(); %>
            <% int i = 0; %>
            <% foreach (var score in highScore.GetCurrentHighScores())
               { %>
            <tr>
                <td>
                    <% i++; %>
                    <%= i.ToString() %>
                </td>
                <td>
                    <%= score.Name %>
                </td>
                <td>
                    <%= score.Country %>
                </td>
                <td>
                    <%= score.Score.ToString() %>
                </td>
            </tr>
            <% } %>
        </table>
    </div>
    </form>
</body>
</html>
