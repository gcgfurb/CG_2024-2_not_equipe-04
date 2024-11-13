using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace gcgcg
{
  internal class Poligono : Objeto
  { 
    
    public Poligono(Objeto _paiRef, ref char _rotulo, List<Ponto4D> pontosPoligono) : base(_paiRef, ref _rotulo)
    {
      PrimitivaTipo = PrimitiveType.LineLoop;
      PrimitivaTamanho = 1;
      base.pontosLista = pontosPoligono;
      Atualizar();
    }

    private void Atualizar()
    {
      base.ObjetoAtualizar();
    }

    public void RemoverPoligono()
    {
      base.PontosApagar();
      Atualizar();
      OnUnload();                  
    }

    public void AdicionarPonto(Ponto4D ponto)
    {
      pontosLista.Add(ponto);
      ObjetoAtualizar();
    }

    public void AlternarAbertoFechado()
    {
      if (PrimitivaTipo == PrimitiveType.LineLoop)
      {
        PrimitivaTipo = PrimitiveType.LineStrip;
      }
      else
      {
        PrimitivaTipo = PrimitiveType.LineLoop; 
      }
      Atualizar(); 
    }

    public int ObterIndiceVerticeMaisProximo(Ponto4D pontoReferencia)
    {
      double menorDistancia = double.MaxValue;
      int indiceVerticeMaisProximo = -1;

      for (int i = 0; i < PontosListaTamanho; i++)
      {
        Ponto4D vertice = PontosId(i);
        double distancia = Math.Sqrt(Math.Pow(vertice.X - pontoReferencia.X, 2) + Math.Pow(vertice.Y - pontoReferencia.Y, 2));

        if (distancia < menorDistancia)
        {
          menorDistancia = distancia;
          indiceVerticeMaisProximo = i;
        }
      }

      return indiceVerticeMaisProximo;
    }

    public void MoverVerticeMaisProximo(Ponto4D pontoReferencia)
    {
      int indiceVerticeMaisProximo = ObterIndiceVerticeMaisProximo(pontoReferencia);

      if (indiceVerticeMaisProximo >= 0)
      {
        PontosAlterar(pontoReferencia, indiceVerticeMaisProximo);
        Atualizar();
      }
    }

    public void RemoverVerticeMaisProximo(Ponto4D pontoReferencia)
    {
      int indiceVerticeMaisProximo = ObterIndiceVerticeMaisProximo(pontoReferencia);

      if (indiceVerticeMaisProximo >= 0)
      {
        pontosLista.RemoveAt(indiceVerticeMaisProximo);
        Atualizar();
      }
    }

    public bool PontoEstaDentro(Ponto4D ponto)
    {
        Ponto4D pontoLocal = MatrizGlobalInversa(ponto);

        Transformacao4D matrizGlobalAtual = ObjetoMatrizGlobal(new Transformacao4D());
        Bbox().Atualizar(matrizGlobalAtual, pontosLista);

        if (!Matematica.Dentro(Bbox(), pontoLocal))
            return false;

        int interseccao = 0;
        int n = PontosListaTamanho;
        for (int i = 0; i < n; i++)
        {
            Ponto4D ptoIni = PontosId(i);
            Ponto4D ptoFim = PontosId((i + 1) % n);
            if (Matematica.ScanLine(pontoLocal, ptoIni, ptoFim))
                interseccao++;
        }
        return (interseccao % 2 != 0);
    }

    private Transformacao4D GetMatriz()
    {
        var fieldInfo = typeof(Objeto).GetField("matriz", BindingFlags.NonPublic | BindingFlags.Instance);
        return (Transformacao4D)fieldInfo.GetValue(this);
    }

    private void SetMatriz(Transformacao4D value)
    {
        var fieldInfo = typeof(Objeto).GetField("matriz", BindingFlags.NonPublic | BindingFlags.Instance);
        fieldInfo.SetValue(this, value);
    }

    public void Transladar(double tx, double ty, double tz)
    {
        var matrizObjeto = GetMatriz();
        Transformacao4D translacao = new Transformacao4D();
        translacao.AtribuirTranslacao(tx, ty, tz);
        matrizObjeto = matrizObjeto.MultiplicarMatriz(translacao);
        SetMatriz(matrizObjeto);
    }

    public void Escalar(double sx, double sy, double sz)
    {
        Ponto4D centroBBox = Bbox().ObterCentro;

        Transformacao4D transParaOrigem = new Transformacao4D();
        transParaOrigem.AtribuirTranslacao(-centroBBox.X, -centroBBox.Y, -centroBBox.Z);

        Transformacao4D escala = new Transformacao4D();
        escala.AtribuirEscala(sx, sy, sz);

        Transformacao4D transDeVolta = new Transformacao4D();
        transDeVolta.AtribuirTranslacao(centroBBox.X, centroBBox.Y, centroBBox.Z);

        var matrizObjeto = GetMatriz();
        matrizObjeto = transDeVolta.MultiplicarMatriz(escala).MultiplicarMatriz(transParaOrigem).MultiplicarMatriz(matrizObjeto);
        SetMatriz(matrizObjeto);
    }

    public void Rotacionar(double anguloEmGraus)
    {
        Ponto4D centroBBox = Bbox().ObterCentro;

        Transformacao4D transParaOrigem = new Transformacao4D();
        transParaOrigem.AtribuirTranslacao(-centroBBox.X, -centroBBox.Y, -centroBBox.Z);

        Transformacao4D rotacao = new Transformacao4D();
        double anguloEmRadianos = anguloEmGraus * Transformacao4D.DEG_TO_RAD;
        rotacao.AtribuirRotacaoZ(anguloEmRadianos);

        Transformacao4D transDeVolta = new Transformacao4D();
        transDeVolta.AtribuirTranslacao(centroBBox.X, centroBBox.Y, centroBBox.Z);

        var matrizObjeto = GetMatriz();
        matrizObjeto = transDeVolta.MultiplicarMatriz(rotacao).MultiplicarMatriz(transParaOrigem).MultiplicarMatriz(matrizObjeto);
        SetMatriz(matrizObjeto);
    }

#if CG_Debug
    public override string ToString()
    {
      System.Console.WriteLine("__________________________________ \n");
      string retorno;
      retorno = "__ Objeto Poligono _ Tipo: " + PrimitivaTipo + " _ Tamanho: " + PrimitivaTamanho + "\n";
      retorno += base.ImprimeToString();
      return retorno;
    }
#endif
  }
}
